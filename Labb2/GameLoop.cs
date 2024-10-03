using Labb2.Elements;
using System;
using System.Xml.Linq;

namespace Labb2
{
    public class GameLoop
    {
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
            var visibleEnemies = LevelData.Elements
                .OfType<Enemy>() 
                .Where(e => e.IsVisible); 

            Action<Enemy> updateEnemy = enemy =>
                enemy.Update((enemy.Position.X, enemy.Position.Y),
                             (player.Position.X, player.Position.Y));

            visibleEnemies.ToList().ForEach(updateEnemy);
        }
        public static List<LevelElement> UpdateVisibleElements((int x, int y) playerPosition, List<LevelElement> elements)
        {
            int visionRadius = 5;
            elements.ForEach(element =>
            {
                if (element is Enemy enemy)
                {
                    enemy.IsVisible = enemy.HealthPoints > 0 && IsWithinVisionRange(element.Position, playerPosition, visionRadius);
                    return; 
                }
                element.IsVisible = (element is Wall && element.IsVisible) || IsWithinVisionRange(element.Position, playerPosition, visionRadius);
            });
            return elements;
        }
        private static bool IsWithinVisionRange(StructPosition position, (int x, int y) playerPosition, int visionRadius)
        {
            int distanceX = Math.Abs(position.X - playerPosition.x);
            int distanceY = Math.Abs(position.Y - playerPosition.y);
            return distanceX <= visionRadius && distanceY <= visionRadius;
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

            string attackMessage = GeneratePlayerAttackMessage(player, attackDiceThrow, enemy, defenceDiceThrow, damage);
            Console.SetCursorPosition(0, 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(attackMessage);

            if (damage > 0)
            {
                enemy.HealthPoints -= damage;
            }
        }
        private static string GeneratePlayerAttackMessage(Player player, int attackRoll, Enemy enemy, int defenceRoll, int damage)
        {
            if (damage > 0)
            {
                return $"{player.Name} (ATK: {player.AttackDice.ToString()} => {attackRoll}) attacked {enemy.Name} " +
                    $"(DEF: {player.DefenceDice.ToString()} => {defenceRoll}) and dealt {damage} damage!";
            }
            else
            {
                return $"{player.Name} (ATK: {player.AttackDice.ToString()} => {attackRoll}) attacked {enemy.Name} " +
                    $"(DEF: {player.DefenceDice.ToString()} => {defenceRoll}), but did not manage to make any damage.";
            }
        }
        public static void EnemyAttackPlayer(Enemy enemy, Player player)
        {
            int attackDiceThrow = 0;
            string attackMessage = string.Empty;
            int defenceDiceThrow = 0;
            int damage = 0;
            if (enemy is Rat rat)
            {
                attackDiceThrow = rat.AttackDice.Throw();
                defenceDiceThrow = player.DefenceDice.Throw();
                damage = attackDiceThrow - defenceDiceThrow;
                attackMessage = GenerateEnemyAttackMessage(rat, attackDiceThrow, player, defenceDiceThrow, damage);
            }
            else if (enemy is Snake snake)
            {
                attackDiceThrow = snake.AttackDice.Throw();
                defenceDiceThrow = player.DefenceDice.Throw();
                damage = attackDiceThrow - defenceDiceThrow;
                attackMessage = GenerateEnemyAttackMessage(snake, attackDiceThrow, player, defenceDiceThrow, damage);
            }

            Console.SetCursorPosition(0, 2);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 2);
            if (damage > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine(attackMessage);

            HandleDamage(enemy, player, damage);
        }
        private static string GenerateEnemyAttackMessage(Enemy enemy, int attackRoll, Player player, int defenceRoll, int damage)
        {
            if (damage > 0)
            {
                return $"{enemy.Name} (ATK: {enemy.AttackDice.ToString()} => {attackRoll}) attacked {player.Name} " +
                    $"(DEF: {enemy.DefenceDice.ToString()} => {defenceRoll}) and dealt {damage} damage!";
            }
            else
            {
                return $"{enemy.Name} (ATK: {enemy.AttackDice.ToString()} => {attackRoll}) attacked {player.Name} " +
                    $"(DEF: {enemy.DefenceDice.ToString()} => {defenceRoll}), but did not manage to make any damage.";
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
            Console.SetCursorPosition(1, Console.WindowHeight - 7);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(player.Position.X, player.Position.Y);
        }
        public static Player GetPlayer()
        {
            return player;
        }
        public static Enemy GetEnemy(int x, int y)
        {
            var enemy = LevelData.Elements
                        .OfType<Enemy>()  
                        .FirstOrDefault(e => e.Position.X == x && e.Position.Y == y);
            return enemy;
        }
        public static string GameOverMessage()
        {
            string gameOverMessage = "You ran out of health points and died. Game over!";
            return gameOverMessage;
        }
    }
}
