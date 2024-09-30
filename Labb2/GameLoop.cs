using Labb2.Elements;
using System.Reflection.Metadata.Ecma335;

namespace Labb2
{
    public class GameLoop
    {
        private List<LevelElement> enemies;
        public static int Moves { get; set; } 
        public GameLoop(Player player, List<LevelElement> enemies)
        {
            Moves++;
            this.enemies = enemies;
            MovePlayer(player);
            MoveEnemies(player);
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
        private void QuitGame(Player player)
        {
            Console.SetCursorPosition(1, Console.WindowHeight - 10);
            
            // ToDo: Fix the printing bugg, first letter is not printed
            string endMessage = "xAre you sure? Press Enter to end the game.";
            Console.WriteLine(endMessage);
            
            ConsoleKey endKey = Console.ReadKey().Key;
            if (endKey == ConsoleKey.Enter)
            {
                Environment.Exit(0);
            }
            Console.SetCursorPosition(player.Position.X, player.Position.Y);
        }
        public void MoveEnemies(Player player)
        {
            foreach (var enemy in enemies)
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
        //public void CheckCollision(Player player)
        //{
        //    foreach (var enemy in enemies)
        //    {
        //        if (enemy.Position.X == player.Position.X && 
        //            enemy.Position.Y == player.Position.Y)
        //        {
        //            if (enemy is Rat)
        //            {
        //                var rat = (Rat)enemy;
        //                player.HealthPoints -= rat.AttackDice.Throw();
        //            }
        //            else if (enemy is Snake)
        //            {
        //                var snake = (Snake)enemy;
        //                player.HealthPoints -= snake.AttackDice.Throw();
        //            }
        //        }
        //    }
        //}
    }
}
