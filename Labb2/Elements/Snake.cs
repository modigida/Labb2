namespace Labb2.Elements
{
    public class Snake : Enemy
    {
        public Snake(StructPosition position) : base(position, "Snake", new Dice(3, 4, 2), new Dice(1, 8, 5))
        {
            Character = 'S';
            Color = ConsoleColor.Green;
            HealthPoints = 25; 
        }
        public override void Update(StructPosition snakePosition, StructPosition playerPosition)
        {
            var snakePos = new StructPosition(snakePosition.X, snakePosition.Y);
            var playerPos = new StructPosition(playerPosition.X, playerPosition.Y);
            var previousPosition = Position;

            snakePos = MoveSnakeAwayFromPlayer(snakePos, playerPos);

            if (!IsElement<LevelElement>(snakePos))
            {
                Position = new StructPosition(snakePos.X, snakePos.Y);
            }

            Draw(previousPosition);
        }
        private StructPosition MoveSnakeAwayFromPlayer(StructPosition snake, StructPosition player)
        {
            if (snake.DistanceTo(player) <= 2)
            {
                snake.X += (player.X < snake.X) ? 1 : (player.X > snake.X) ? -1 : 0;
                snake.Y += (player.Y < snake.Y) ? 1 : (player.Y > snake.Y) ? -1 : 0;
            }
            return snake;
        }
    }
}
