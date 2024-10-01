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
            foreach (var enemy in Enemies)
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
            int attack = player.AttackDice.Throw();
            int defence = enemy.DefenceDice.Throw();
            int damage = attack - defence;

            if (damage > 0)
            {
                enemy.HealthPoints -= damage;
                Console.SetCursorPosition(0, 1);
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
        public static void RatAttackPlayer(Rat rat, Player player)
        {
            int attack = rat.AttackDice.Throw();
            int defence = player.DefenceDice.Throw();
            int damage = attack - defence;

            if (damage > 0)
            {
                player.HealthPoints -= damage;
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"You attacked the {player.Name} and dealt {damage} damage. Current HP is {player.HealthPoints}");
                if (player.HealthPoints <= 0)
                {
                    // Game over
                    Console.WriteLine(GameOverMessage());
                }
            }
            else if (damage < 0)
            {
                rat.HealthPoints -= Math.Abs(damage);
                if (rat.HealthPoints <= 0)
                {
                    Enemies.Remove(rat);
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
        public static string GameOverMessage()
        {
            string gameOverMessage = "You ran out of health points and died. Game over!";
            return gameOverMessage;
        }
    }
}
