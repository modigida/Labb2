namespace Labb2.Elements
{
    public abstract class LevelElement
    {
        public StructPosition Position { get; set; }
        public char Character { get; set; }
        public ConsoleColor Color { get; set; }
        public bool IsVisible { get; set; }
        public LevelElement(StructPosition position)
        {
            Position = position;
        }
        public void Draw()
        {
            if (!TrySetCursorPosition(Position.X, Position.Y)) return;

            SetConsoleColors();
            Console.Write(Character);
            Console.ResetColor();
        }
        public void Draw(StructPosition previousPosition)
        {
            TrySetCursorPosition(previousPosition.X, previousPosition.Y, ' ');

            int positionX = Position.X;
            int positionY = Position.Y;

            if (TrySetCursorPosition(positionX, positionY))
            {
                SetConsoleColors();
                Console.Write(Character);
                Console.ResetColor();
            }
        }
        private void SetConsoleColors()
        {
            if (IsVisible)
            {
                if (Character == '#')
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                Console.ForegroundColor = Color;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }
        private bool TrySetCursorPosition(int x, int y, char? clearChar = null)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                if (clearChar.HasValue)
                {
                    Console.Write(clearChar.Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsElement<T>(int x, int y) where T : LevelElement
        {
            return LevelData.Elements
                    .OfType<T>()
                    .Any(element => element.Position.X == x && element.Position.Y == y);
        }
    }
}
