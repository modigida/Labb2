using Labb2.Elements;
using System;
using System.Xml.Linq;

namespace Labb2
{
    public class GameLoop
    {
        public static List<LevelElement> Enemies = new List<LevelElement>();
        public static int Moves { get; set; }
        private static Player player;
        public GameLoop(Player player)
        {
            Moves++;
            GameLoop.player = player;
            MovePlayer(player);
            MoveEnemies(player);
            Console.WriteLine(player.ToString());
        }
        public void MovePlayer(Player player)
        {
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow ||
                key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
            {
                player.Move(key);
            }
            else if (key == ConsoleKey.Escape)
            {
                QuitGame(player);
            }
        }
        public void MoveEnemies(Player player)
        {
            var enemiesCopy = Enemies.ToList();
            foreach (var enemy in enemiesCopy)
            {
                if (enemy is Rat)
                {
                    var rat = (Rat)enemy;
                    rat.Update((rat.Position.X, rat.Position.Y),
                        (player.Position.X, player.Position.Y));
                }
                else if (enemy is Snake)
                {
                    var snake = (Snake)enemy;
                    snake.Update((snake.Position.X, snake.Position.Y),
                        (player.Position.X, player.Position.Y));
                }
            }
        }
        public static List<LevelElement> UpdateVisibleElements((int x, int y) playerPosition, List<LevelElement> elements)
        {
            int visionRadius = 5;

            foreach (var element in elements)
            {
                int distanceX = Math.Abs(element.Position.X - playerPosition.x);
                int distanceY = Math.Abs(element.Position.Y - playerPosition.y);

                if (distanceX <= visionRadius && distanceY <= visionRadius)
                {
                    element.IsVisible = true;
                }
                else if (element is Wall && element.IsVisible)
                {
                    element.IsVisible = true;
                }
                else
                {
                    element.IsVisible = false;
                }
            }
            return elements;
        }
        public static void PlayerAttackEnemy(Player player, int x, int y)
        {
            var enemy = GetEnemy(x, y); 

            if (enemy != null) 
            {
                if (enemy is Rat rat)
                {
                    ExecuteAttack(player, rat);
                }
                else if (enemy is Snake snake)
                {
                    ExecuteAttack(player, snake);
                }
            }
        }
        private static void ExecuteAttack(Player player, Enemy enemy)
        {
            int attackDiceThrow = player.AttackDice.Throw();
            int defenceDiceThrow = enemy.DefenceDice.Throw();
            int damage = attackDiceThrow - defenceDiceThrow;

            string attackDiceConfiguratior = $"{player.AttackDice.NumberOfDice}d{player.AttackDice.SidesPerDice}+{player.AttackDice.Modifier}";
            string defenceDiceConfiguratior = $"{enemy.DefenceDice.NumberOfDice}d{enemy.DefenceDice.SidesPerDice}+{enemy.DefenceDice.Modifier}";
            string attackMessage = GeneratePlayerAttackMessage(player, attackDiceThrow, attackDiceConfiguratior, 
                enemy, defenceDiceThrow, defenceDiceConfiguratior, damage);
            Console.SetCursorPosition(0, 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 1);
            if (damage > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(attackMessage);

            if (damage > 0)
            {
                enemy.HealthPoints -= damage;
                Console.SetCursorPosition(0, 2);
                if (enemy.HealthPoints <= 0)
                {
                    Enemies.Remove(enemy);
                }
            }
        }
        private static string GeneratePlayerAttackMessage(Player player, int attackRoll, string attackDiceConfig, 
            Enemy enemy, int defenceRoll, string defenceDiceConfig, int damage)
        {
            if (damage > 0)
            {
                return $"{player.Name} (ATK: {attackDiceConfig} => {attackRoll}) attacked the {enemy.Name} " +
                    $"(DEF: {defenceDiceConfig} => {defenceRoll}) and dealt {damage} damage!";
            }
            else
            {
                return $"{player.Name} (ATK: {attackDiceConfig} => {attackRoll}) attacked the {enemy.Name} " +
                    $"(DEF: {defenceDiceConfig} => {defenceRoll}), but did not manage to make any damage.";
            }
        }
        public static void EnemyAttackPlayer(Enemy enemy, Player player)
        {
            int attackDiceThrow = 0;
            string attackDiceConfiguratior = string.Empty; 
            string defenceDiceConfiguratior = string.Empty;
            string attackMessage = string.Empty;
            int defenceDiceThrow = 0;
            int damage = 0;
            if (enemy is Rat rat)
            {
                attackDiceThrow = rat.AttackDice.Throw();
                attackDiceConfiguratior = $"{rat.AttackDice.NumberOfDice}d{rat.AttackDice.SidesPerDice}+{rat.AttackDice.Modifier}";
                defenceDiceConfiguratior = $"{rat.DefenceDice.NumberOfDice}d{rat.DefenceDice.SidesPerDice}+{rat.DefenceDice.Modifier}";
                defenceDiceThrow = player.DefenceDice.Throw();
                damage = attackDiceThrow - defenceDiceThrow;
                attackMessage = GenerateEnemyAttackMessage(rat, attackDiceThrow, attackDiceConfiguratior, player, defenceDiceThrow, 
                    defenceDiceConfiguratior, damage);
            }
            else if (enemy is Snake snake)
            {
                attackDiceThrow = snake.AttackDice.Throw();
                attackDiceConfiguratior = $"{snake.AttackDice.NumberOfDice}d{snake.AttackDice.SidesPerDice}+{snake.AttackDice.Modifier}";
                defenceDiceConfiguratior = $"{snake.DefenceDice.NumberOfDice}d{snake.DefenceDice.SidesPerDice}+{snake.DefenceDice.Modifier}";
                defenceDiceThrow = player.DefenceDice.Throw();
                damage = attackDiceThrow - defenceDiceThrow;
                attackMessage = GenerateEnemyAttackMessage(snake, attackDiceThrow, attackDiceConfiguratior, player, defenceDiceThrow, 
                    defenceDiceConfiguratior, damage);
            }

            Console.SetCursorPosition(0, 2);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 2);
            if (damage > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(attackMessage);

            HandleDamage(enemy, player, damage);
        }
        private static string GenerateEnemyAttackMessage(Enemy enemy, int attackRoll, string attackDiceConfig,
            Player player, int defenceRoll, string defenceDiceConfig, int damage)
        {
            if (damage > 0)
            {
                return $"{enemy.Name} (ATK: {attackDiceConfig} => {attackRoll}) attacked the {player.Name} " +
                    $"(DEF: {defenceDiceConfig} => {defenceRoll}) and dealt {damage} damage!";
            }
            else
            {
                return $"{enemy.Name} (ATK: {attackDiceConfig} => {attackRoll}) attacked the {player.Name} " +
                    $"(DEF: {defenceDiceConfig} => {defenceRoll}), but did not manage to make any damage.";
            }
        }
        public static void HandleDamage(Enemy enemy, Player player, int damage) 
        { 
            if (damage > 0)
            {
                player.HealthPoints -= damage;
                if (player.HealthPoints <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, Console.WindowHeight - 7);
                    Console.WriteLine(GameOverMessage());
                    Environment.Exit(0);
                }
            }
        }
        private void QuitGame(Player player)
        {
            Console.SetCursorPosition(1, Console.WindowHeight - 7);

            // ToDo: Fix the printing bugg, first letter is not printed
            string endMessage = "xAre you sure? Press Enter to end the game.";
            Console.WriteLine(endMessage);

            ConsoleKey endKey = Console.ReadKey().Key;
            if (endKey == ConsoleKey.Enter)
            {
                Console.SetCursorPosition(1, Console.WindowHeight - 5);
                string finalMessage = "Spelet är avslutat. Tack för att du spelade. Välkommen åter!";
                Console.WriteLine(finalMessage);

                Environment.Exit(0);
            }
            Console.SetCursorPosition(player.Position.X, player.Position.Y);
        }
        public static Player GetPlayer()
        {
            return player;
        }
        public static Enemy GetEnemy(int x, int y)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy is Rat rat)
                {
                    if (rat.Position.X == x && rat.Position.Y == y)
                    {
                        return rat;
                    }
                }
                else if (enemy is Snake snake)
                {
                    if (snake.Position.X == x && snake.Position.Y == y)
                    {
                        return snake;
                    }
                }
            }
            return null;
        }
        public static string GameOverMessage()
        {
            string gameOverMessage = "You ran out of health points and died. Game over!";
            return gameOverMessage;
        }
    }
}
