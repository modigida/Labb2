using Labb2.Elements;

namespace Labb2
{
    public class GameLoop
    {
        public static List<LevelElement> Enemies = new List<LevelElement>();
        public static int Moves { get; set; }
        public GameLoop(Player player)
        {
            Moves++;
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
                Console.SetCursorPosition(1, Console.WindowHeight - 10);
                string finalMessage = "Spelet är avslutat. Tack för att du spelade. Välkommen åter!";
                Console.WriteLine(finalMessage);

                Environment.Exit(0);
            }
            Console.SetCursorPosition(player.Position.X, player.Position.Y);
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
            foreach (var enemy in Enemies)
            {
                if (enemy.Position.X == x && enemy.Position.Y == y)
                {
                    if (enemy is Rat rat)
                    {
                        int attack = player.AttackDice.Throw();
                        int defence = rat.DefenceDice.Throw();
                        int damage = attack - defence;
                        if (damage > 0)
                        {
                            rat.HealthPoints -= damage;
                        }
                        else if (damage < 0)
                        {
                            player.HealthPoints -= Math.Abs(damage);
                        }
                    }
                    else if (enemy is Snake snake)
                    {
                        int attack = player.AttackDice.Throw();
                        int defence = snake.DefenceDice.Throw();
                        int damage = attack - defence;
                        if (damage > 0)
                        {
                            snake.HealthPoints -= damage;
                        }
                        else if (damage < 0)
                        {
                            player.HealthPoints -= Math.Abs(damage);
                        }
                    }
                }
            }
            
        }
        public static void RatAttackPlayer()
        {
            // Not needed for snake to be able to attack player sinse snake only moves away from player
        }

    }
}
