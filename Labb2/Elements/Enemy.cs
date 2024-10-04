using System.Xml.Linq;

namespace Labb2.Elements
{
    public abstract class Enemy : LevelElement
    {
        public string Name { get; set; }
        public int HealthPoints { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }
        //protected Enemy(StructPosition position) : base(position)
        //{
        //
        //}
        protected Enemy(StructPosition position, string name, Dice attackDice, Dice defenceDice) : base(position)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AttackDice = attackDice ?? throw new ArgumentNullException(nameof(attackDice));
            DefenceDice = defenceDice ?? throw new ArgumentNullException(nameof(defenceDice));
        }
        public abstract void Update(StructPosition enemyPosition, StructPosition playerPosition);
    }
}
