namespace Labb2.Elements
{
    public class Snake : Enemy
    {
        public Snake(StructPosition position) : base(position)
        {
            Character = 'S';
            Color = ConsoleColor.Green;
            Name = "Snake";
            HealthPoints = 25;
            AttackDice = new Dice(3, 4, 2);
            DefenceDice = new Dice(1, 8, 5);
        }
        public override void Update((int x, int y) snakePosition, (int x, int y) playerPosition)
        {
            var snakePos = new StructPosition(snakePosition.x, snakePosition.y);
            var playerPos = new StructPosition(playerPosition.x, playerPosition.y);
            double distanceToPlayer = snakePos.DistanceTo(playerPos);
            var previousPosition = Position;

            if (distanceToPlayer < 2)
            {
                if (snakePosition.x < playerPosition.x)
                {
                    snakePosition.x -= 1;
                }
                else if (snakePosition.x > playerPosition.x)
                {
                    snakePosition.x += 1;
                }
                if (snakePosition.y < playerPosition.y)
                {
                    snakePosition.y -= 1;
                }
                else if (snakePosition.y > playerPosition.y)
                {
                    snakePosition.y += 1;
                }
                if (!IsWall(snakePosition.x, snakePosition.y) && 
                    !IsPlayer(snakePosition.x, snakePosition.y, playerPosition.x, playerPosition.y) &&
                    !IsEnemy(snakePosition.x, snakePosition.y))
                {
                    Position = new StructPosition(snakePosition.x, snakePosition.y);
                }
            }
            Draw(previousPosition);
        }
    }
}
