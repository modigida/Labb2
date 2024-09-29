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
    foreach (var element in elementList)
    {
        Console.WriteLine($"Element position: ({element.Character}, {element.Position.X}, {element.Position.Y})");
    }
}
else
{
    Console.WriteLine("File not found");
}

bool isRunning = true;

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

    // This function delays the players movement since it reads a keypress
    ConsoleKey key = Console.ReadKey().Key;
    if (key == ConsoleKey.Escape)
    {
        Console.SetCursorPosition(1, Console.WindowHeight - 10);

        // ToDo: Fix the printing bugg, first letter is not printed
        string endMessage = "xAre you sure? Press Enter to end the game.";
        Console.WriteLine(endMessage);
        
        ConsoleKey endKey = Console.ReadKey().Key;
        if (endKey == ConsoleKey.Enter)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }
        Console.SetCursorPosition(player.Position.X, player.Position.Y);
    }
}
while (isRunning);

Console.SetCursorPosition(1, Console.WindowHeight - 10);
string finalMessage = "Spelet är avslutat. Tack för att du spelade. Välkommen åter!";
Console.WriteLine(finalMessage);
string amountOfGameLoops = $"Antal drag: {GameLoop.Moves}";
Console.WriteLine(amountOfGameLoops);
