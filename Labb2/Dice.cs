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
            int total = 0;
            for (int i = 0; i < NumberOfDice; i++)
            {
                total += random.Next(1, SidesPerDice + 1); 
            }
            total += Modifier;
            return total;
        }
        public override string ToString()
        {
            string diceConfiguration = $"{NumberOfDice}d{SidesPerDice}+{Modifier}";
            return diceConfiguration;
        }
    }
}
