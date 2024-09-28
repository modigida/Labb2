namespace Labb2.Elements
{
    public abstract class Enemy : LevelElement
    {
        public string Name { get; set; }
        public int HealthPoints { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }

        protected Enemy(StructPosition position) : base(position)
        {
            
        }
        public abstract void Update((int x, int y) enemyPosition, (int x, int y) playerPosition);
    }
}
