namespace Labb2.Elements
{
    public abstract class LevelElement
    {
        public StructPosition Position { get; set; }
        public char Character { get; set; }
        public ConsoleColor Color { get; set; }
        public bool IsVisible { get; set; }
        public static List<LevelElement> Walls = new List<LevelElement>();
        public static List<LevelElement> Enemies = new List<LevelElement>();
        public LevelElement(StructPosition position)
        {
            Position = position;
        }
        public void Draw()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            if (IsVisible)
            {
                if (Character == '#')
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = Color;
                }
                Console.ForegroundColor = Color;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.Write(Character);
            Console.ResetColor();
        }
        public void Draw(StructPosition previousPosition)
        {
            var positionX = Position.X;
            var positionY = Position.Y;

            try
            {
                Console.SetCursorPosition(previousPosition.X, previousPosition.Y);
                Console.Write(" ");
            }
            catch
            {
                positionX = previousPosition.X;
                positionY = previousPosition.Y;
            }
            try
            {
                Console.SetCursorPosition(positionX, positionY);
                if(IsVisible)
                {
                    if(Character == '#')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = Color;
                    }
                    Console.ForegroundColor = Color;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(Character);
                Console.ResetColor();
            }
            catch
            {
                positionX = previousPosition.X;
                positionY = previousPosition.Y;
            }
        }
        public bool IsWall(int x, int y)
        {
            if (Walls.Any((wall) => wall.Position.X == x && wall.Position.Y == y))
            {
                return true;
            }
            return false; 
        }
        public bool IsEnemie(Player player, int x, int y)
        {
            if (Enemies.Any(enemy => enemy.Position.X == x && enemy.Position.Y == y))
            {
                return true;
            }
            return false;
        }
        public bool IsPlayer(int x, int y, int playerPositionX, int playerPositionY)
        {
            if (playerPositionX == x && playerPositionY == y)
            {
                return true;
            }
            return false;
        }
    }
}
