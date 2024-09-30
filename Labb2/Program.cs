using Labb2;
using Labb2.Elements;

string startMessage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProgramDescription", "DescriptionAndRules.txt");
if (File.Exists(startMessage))
{
    string fileContent = File.ReadAllText(startMessage);
    Console.WriteLine(fileContent);
}
else
{
    Console.WriteLine("File not found");
}

while (true)
{
    ConsoleKey enterKey = Console.ReadKey().Key;
    if (enterKey == ConsoleKey.Enter)
    {
        Console.Clear();
        break;
    }
    else
    {
        Console.WriteLine("Press Enter to continue.");
    }
}

var leveldata = new LevelData();

string textfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels", "Level1.txt");

List<LevelElement> elementList = new List<LevelElement>(); 

if (File.Exists(textfile))
{
    elementList = leveldata.Load(textfile);
}
else
{
    Console.WriteLine("File not found");
}

var player = new Player(new StructPosition(1, 1));

int maxX = elementList.Max(element => element.Position.X);
int maxY = elementList.Max(element => element.Position.Y);

do
{
    var game = new GameLoop(player, elementList);
    Console.Clear();
    player.Draw();
    GameLoop.UpdateVisibleElements((player.Position.X, player.Position.Y), elementList);
    LevelData.Print(maxX + 1, maxY + 1, elementList);
    // Loop will end when game over or player wins
}
while (true);

Console.SetCursorPosition(1, Console.WindowHeight - 10);
string finalMessage = "Spelet är avslutat. Tack för att du spelade. Välkommen åter!";
Console.WriteLine(finalMessage);
string amountOfGameLoops = $"Antal drag: {GameLoop.Moves}";
Console.WriteLine(amountOfGameLoops);
