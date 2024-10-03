namespace Labb2.Elements
{
    public class Rat : Enemy
    {
        public Rat(StructPosition position) : base(position)
        {
            Character = 'R';
            Color = ConsoleColor.Red;
            Name = "Rat";
            HealthPoints = 10;
            AttackDice = new Dice(1, 6, 3);
            DefenceDice = new Dice(1, 6, 1);
        }
        public override void Update((int x, int y) ratPosition, (int x, int y) playerPosition)
        {
            Random random = new Random();
            int direction = random.Next(4);
            var previousPosition = Position;
            switch (direction)
            {
                case 0:
                    ratPosition.x -= 1;
                    break;
                case 1:
                    ratPosition.x += 1;
                    break;
                case 2:
                    ratPosition.y -= 1;
                    break;
                case 3:
                    ratPosition.y += 1;
                    break;
            }
            if ((ratPosition.x < 53 && ratPosition.y < 18) &&
                !IsElement<LevelElement>(ratPosition.x, ratPosition.y))
            {
                Position = new StructPosition(ratPosition.x, ratPosition.y);
            }
            if (IsElement<Player>(ratPosition.x, ratPosition.y))
            {
                Player player = GameLoop.GetPlayer();
                GameLoop.Attack(this, player);
                GameLoop.Attack(player, this);
            }
            Draw(previousPosition);
        }
    }
}