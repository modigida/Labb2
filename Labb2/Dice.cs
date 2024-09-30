using System.Security.Principal;

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
            Console.SetCursorPosition(0, 0);
            string resultString = "This is the result of the attack.";
            return resultString;
        }
    }
}
