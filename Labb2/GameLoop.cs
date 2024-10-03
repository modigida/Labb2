using Labb2.Elements;

namespace Labb2
{
    public class GameLoop
    {
        public static int Moves { get; set; }
        private static Player player;
        public GameLoop(Player player)
        {
            Moves++;
            GameLoop.player = player;
            MovePlayer(player);
            MoveEnemies(player);
            Console.WriteLine(player.ToString());
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
        public void MoveEnemies(Player player)
        {
            var visibleEnemies = LevelData.Elements
                .OfType<Enemy>() 
                .Where(e => e.IsVisible); 

            Action<Enemy> updateEnemy = enemy =>
                enemy.Update((enemy.Position.X, enemy.Position.Y),
                             (player.Position.X, player.Position.Y));

            visibleEnemies.ToList().ForEach(updateEnemy);
        }
        public static List<LevelElement> UpdateVisibleElements((int x, int y) playerPosition, List<LevelElement> elements)
        {
            int visionRadius = 5;
            elements.ForEach(element =>
            {
                if (element is Enemy enemy)
                {
                    enemy.IsVisible = enemy.HealthPoints > 0 && IsWithinVisionRange(element.Position, playerPosition, visionRadius);
                    return; 
                }
                element.IsVisible = (element is Wall && element.IsVisible) || IsWithinVisionRange(element.Position, playerPosition, visionRadius);
            });
            return elements;
        }
        private static bool IsWithinVisionRange(StructPosition position, (int x, int y) playerPosition, int visionRadius)
        {
            int distanceX = Math.Abs(position.X - playerPosition.x);
            int distanceY = Math.Abs(position.Y - playerPosition.y);
            return distanceX <= visionRadius && distanceY <= visionRadius;
        }
        public static void Attack(LevelElement attacker, LevelElement defender)
        {
            (int attackDiceThrow, int defenceDiceThrow, int damage) = CalculateDamage(attacker, defender);

            HandleAttackMessage(attacker, attackDiceThrow, defender, defenceDiceThrow, damage);

            if (damage > 0)
            {
                ApplyDamage(attacker, defender, damage);
            }
        }

        private static (int attackRoll, int defenceRoll, int damage) CalculateDamage(LevelElement attacker, LevelElement defender)
        {
            int attackDiceThrow = attacker is Player player
                ? player.AttackDice.Throw()
                : ((Enemy)attacker).AttackDice.Throw();

            int defenceDiceThrow = defender is Player
                ? ((Player)defender).DefenceDice.Throw()
                : ((Enemy)defender).DefenceDice.Throw();

            int damage = attackDiceThrow - defenceDiceThrow;

            return (attackDiceThrow, defenceDiceThrow, damage);
        }

        private static void HandleAttackMessage(LevelElement attacker, int attackRoll, LevelElement defender, int defenceRoll, int damage)
        {
            string attackMessage = GenerateAttackMessage(attacker, attackRoll, defender, defenceRoll, damage);

            Console.SetCursorPosition(0, attacker is Player ? 1 : 2);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, attacker is Player ? 1 : 2);
            Console.ForegroundColor = attacker is Player ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.WriteLine(attackMessage);
        }

        private static void ApplyDamage(LevelElement attacker, LevelElement defender, int damage)
        {
            if (attacker is Player)
            {
                ((Enemy)defender).HealthPoints -= damage; 
            }
            else if (attacker is Enemy)
            {
                ((Player)defender).HealthPoints -= damage; 
            }
            HandleDamage(defender, damage);
        }
        private static void HandleDamage(LevelElement defender, int damage)
        {
            if (damage > 0)
            {
                if (defender is Player player)
                {
                    player.HealthPoints -= damage;

                    if (player.HealthPoints <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(0, Console.WindowHeight - 7);
                        Console.WriteLine(GameOverMessage());
                        Environment.Exit(0);
                    }
                }
                else if (defender is Enemy enemy)
                {
                    enemy.HealthPoints -= damage;

                    if (enemy.HealthPoints <= 0)
                    {
                        enemy.IsVisible = false; 
                    }
                }
            }
        }
        private static string GenerateAttackMessage(LevelElement attacker, int attackRoll, LevelElement defender, int defenceRoll, int damage)
        {
            string attackerName = attacker is Player ? (attacker as Player).Name : (attacker as Enemy).Name;
            string defenderName = defender is Player ? (defender as Player).Name : (defender as Enemy).Name;

            string attackerDice = attacker is Player ? (attacker as Player).AttackDice.ToString() : (attacker as Enemy).AttackDice.ToString();
            string defenderDice = defender is Player ? (defender as Player).DefenceDice.ToString() : (defender as Enemy).DefenceDice.ToString();

            if (damage > 0)
            {
                return $"{attackerName} (ATK: {attackerDice} => {attackRoll}) attacked {defenderName} " +
                       $"(DEF: {defenderDice} => {defenceRoll}) and dealt {damage} damage!";
            }
            else
            {
                return $"{attackerName} (ATK: {attackerDice} => {attackRoll}) attacked {defenderName} " +
                       $"(DEF: {defenderDice} => {defenceRoll}), but did not manage to make any damage.";
            }
        }
        private void QuitGame(Player player)
        {
            Console.SetCursorPosition(1, Console.WindowHeight - 7);
            string endMessage = "xAre you sure? Press Enter to end the game.";
            Console.WriteLine(endMessage);

            ConsoleKey endKey = Console.ReadKey().Key;
            if (endKey == ConsoleKey.Enter)
            {
                Console.SetCursorPosition(1, Console.WindowHeight - 5);
                string finalMessage = "Spelet är avslutat. Tack för att du spelade. Välkommen åter!";
                Console.WriteLine(finalMessage);

                Environment.Exit(0);
            }
            Console.SetCursorPosition(1, Console.WindowHeight - 7);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(player.Position.X, player.Position.Y);
        }
        public static Player GetPlayer()
        {
            return player;
        }
        public static Enemy? GetEnemy(int x, int y)
        {
            return LevelData.Elements
                .OfType<Enemy>()
                .FirstOrDefault(e => e.Position.X == x && e.Position.Y == y);
        }
        public static string GameOverMessage()
        {
            string gameOverMessage = "You ran out of health points and died. Game over!";
            return gameOverMessage;
        }
    }
}
