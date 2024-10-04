namespace Labb2.Elements
{
    public class Rat : Enemy
    {
        public Rat(StructPosition position) : base(position, "Rat", new Dice(1, 6, 3), new Dice(1, 6, 1))
        {
            Character = 'R';
            Color = ConsoleColor.Red;
            HealthPoints = 10;
        }
        public override void Update(StructPosition ratPosition, StructPosition playerPosition)
        {
            Random random = new Random();
            int direction = random.Next(4);
            var previousPosition = Position;
            ratPosition = MoveRatRandomly(ratPosition, direction);
            if ((ratPosition.X < 53 && ratPosition.Y < 19) &&
                !IsElement<LevelElement>(ratPosition))
            {
                Position = new StructPosition(ratPosition.X, ratPosition.Y);
            }
            if (IsElement<Player>(ratPosition))
            {
                Player? player = GameLoop.GetPlayer();
                GameLoop.Attack(this, player!);
                GameLoop.Attack(player!, this);
            }
            Draw(previousPosition);
        }
        private StructPosition MoveRatRandomly(StructPosition ratPosition, int direction)
        {
            switch (direction)
            {
                case 0:
                    ratPosition.X -= 1;
                    break;
                case 1:
                    ratPosition.X += 1;
                    break;
                case 2:
                    ratPosition.Y -= 1;
                    break;
                case 3:
                    ratPosition.Y += 1;
                    break;
            }
            return ratPosition;
        }
    }
}