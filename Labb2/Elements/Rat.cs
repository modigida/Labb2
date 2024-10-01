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
            if (!IsWall(ratPosition.x, ratPosition.y) && 
                !IsPlayer(ratPosition.x, ratPosition.y, playerPosition.x, playerPosition.y) && 
                !IsEnemy(ratPosition.x, ratPosition.y) &&
                (ratPosition.x < 53 && ratPosition.y < 18))
            {
                Position = new StructPosition(ratPosition.x, ratPosition.y);
            }
            if (IsPlayer(ratPosition.x, ratPosition.y, playerPosition.x, playerPosition.y))
            {
                Player player = GameLoop.GetPlayer();
                GameLoop.RatAttackPlayer(this, player);
            }
            Draw(previousPosition);
        }
    }
}