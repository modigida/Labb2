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
            HealthPoints = 100;
            AttackDice = new Dice(2, 6, 2);
            DefenceDice = new Dice(2, 6, 0);
        }
        public void Move(ConsoleKey key)
        {
            var previousPosition = Position;
            var newPosition = MovePlayer(key, Position);
            if (!IsElement<LevelElement>(newPosition))
            {
                Position = new StructPosition(newPosition.X, newPosition.Y);
            }
            if (IsElement<Enemy>(newPosition))
            {
                var enemy = GameLoop.GetEnemy(newPosition)!;
                if (!enemy.IsVisible)
                {
                    Position = new StructPosition(newPosition.X, newPosition.Y);
                }
                if (enemy.IsVisible)
                {
                    GameLoop.Attack(this, enemy);
                    GameLoop.Attack(enemy, this);
                }
            }
            Draw(previousPosition);
        }
        public StructPosition MovePlayer(ConsoleKey key, StructPosition playerPosition)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    playerPosition.X -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    playerPosition.X += 1;
                    break;
                case ConsoleKey.UpArrow:
                    playerPosition.Y -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    playerPosition.Y += 1;
                    break;
            }
            return playerPosition;
        }
        public override string ToString()
        {
            Console.SetCursorPosition(0, 0);
            string nameSection = $"Name: {Name}".PadRight(14);
            string healthSection = $"Health: {HealthPoints}/100".PadRight(17).PadLeft(19);
            string turnSection = $"Turn: {GameLoop.Moves}".PadLeft(10);

            string playerString = $"{nameSection} - {healthSection} - {turnSection}";
            return playerString;
        }
    }
}
