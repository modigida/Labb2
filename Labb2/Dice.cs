using Labb2.Elements;

namespace Labb2
{
    public class Dice
    {
        public int NumberOfDice { get; set; }
        public int SidesPerDice { get; set; }
        public int Modifier { get; set; }
        public Dice(int numberOfDice, int sidesPerDice, int modifier)
        {
            NumberOfDice = numberOfDice;
            SidesPerDice = sidesPerDice;
            Modifier = modifier;
        }
        public int Throw()
        {
            Random random = new Random();
            int total = Enumerable.Range(0, NumberOfDice)
                                  .Sum(_ => random.Next(1, SidesPerDice + 1));
            return total + Modifier;
        }
        public override string ToString() => $"{NumberOfDice}d{SidesPerDice}+{Modifier}";
    }
}
