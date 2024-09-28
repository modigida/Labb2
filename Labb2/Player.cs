using Labb2.Elements;

namespace Labb2
{
    public class Player : LevelElement
    {
        public string Name { get; set; }
        public int HealthPoints { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }
        public Player(StructPosition position) : base(position)
        {
            Character = 'P';
            Color = ConsoleColor.DarkYellow;
            IsVisible = true;
            Name = "Player";
            HealthPoints = 25;
            AttackDice = new Dice(2, 6, 2);
            DefenceDice = new Dice(2, 6, 0);
        }
        public void Move(ConsoleKey key)
        {
            var previousPosition = Position;

            var newPosition = MovePlayer(key, (Position.X, Position.Y));
            
            if (!IsWall(newPosition.x, newPosition.y) && !IsEnemie(newPosition.x, newPosition.y))
            {
                Position = new StructPosition(newPosition.x, newPosition.y);
            }
            Draw(previousPosition);
        }
        public (int x, int y) MovePlayer(ConsoleKey key, (int x, int y) playerPosition)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    playerPosition.x -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    playerPosition.x += 1;
                    break;
                case ConsoleKey.UpArrow:
                    playerPosition.y -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    playerPosition.y += 1;
                    break;
            }
            return playerPosition;
        }
    }
}
