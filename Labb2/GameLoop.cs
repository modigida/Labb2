using Labb2.Elements;
using System;

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
            var enemy = Enemies.FirstOrDefault(e => e.Position.X == x && e.Position.Y == y);

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
            string attackMessage = GenerateAttackMessage(player, attackDiceThrow, attackDiceConfiguratior, 
                enemy, defenceDiceThrow, defenceDiceConfiguratior, damage);
            Console.SetCursorPosition(0, 1);
            if(damage > 0)
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
                Console.WriteLine($"You attacked the {enemy.Name} and dealt {damage} damage. Current HP is {enemy.HealthPoints}");
                if (enemy.HealthPoints <= 0)
                {
                    Enemies.Remove(enemy);
                }
            }
            else if (damage < 0)
            {
                player.HealthPoints -= Math.Abs(damage);
                if (player.HealthPoints <= 0)
                {
                    Console.WriteLine(GameOverMessage());
                    // Skapa game over-funktion
                }
            }
        }
        private static string GenerateAttackMessage(Player player, int attackRoll, string attackDiceConfig, 
            Enemy enemy, int defenceRoll, string defenceDiceConfig, int damage)
        {
            if (damage > 0)
            {
                return $"You (ATK: {attackDiceConfig} => {attackRoll}) attacked the {enemy.Name} " +
                    $"(DEF: {defenceDiceConfig} => {defenceRoll}) and dealt {damage} damage!";
            }
            else
            {
                return $"You (ATK: {attackDiceConfig} => {attackRoll}) attacked the {enemy.Name} " +
                    $"(DEF: {defenceDiceConfig} => {defenceRoll}), but did not manage to make any damage.";
            }
        }
        public static void EnemyAttackPlayer(Enemy enemy, Player player)
        {
            int attack = 0;
            if (enemy is Rat rat)
            {
                attack = rat.AttackDice.Throw();

            }
            else if (enemy is Snake snake)
            {
                attack = snake.AttackDice.Throw();
            }
            int defence = player.DefenceDice.Throw();
            int damage = attack - defence;
            HandleDamage(enemy, player, damage);
        }
        public static void HandleDamage(Enemy enemy, Player player, int damage) 
        { 
            if (damage > 0)
            {
                player.HealthPoints -= damage;
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"You attacked the {player.Name} and dealt {damage} damage. Current HP is {player.HealthPoints}");
                if (player.HealthPoints <= 0)
                {
                    // Game over
                    Console.WriteLine(GameOverMessage());
                    Environment.Exit(0);
                }
            }
            else if (damage < 0)
            {
                if (enemy is Rat rat)
                {
                    rat.HealthPoints -= Math.Abs(damage);
                    if (rat.HealthPoints <= 0)
                    {
                        Enemies.Remove(rat);
                    }
                }
                else if (enemy is Snake snake)
                {
                    snake.HealthPoints -= Math.Abs(damage);
                    if (snake.HealthPoints <= 0)
                    {
                        Enemies.Remove(snake);
                    }
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
