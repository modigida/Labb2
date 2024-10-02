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

if (File.Exists(textfile))
{
    leveldata.Load(textfile);
}
else
{
    Console.WriteLine("File not found");
}

var player = new Player(new StructPosition(1, 4));

int maxX = LevelData.Elements.Max(element => element.Position.X);
int maxY = LevelData.Elements.Max(element => element.Position.Y);
GameLoop.Enemies = LevelData.Elements.Where(element => element is Enemy).ToList();
do
{
    var game = new GameLoop(player);
    // Why do I need to press key to continue here?
    int startRow = 3; 
    int numRowsToClear = 20;
    for (int i = 0; i < numRowsToClear; i++)
    {
        Console.SetCursorPosition(0, startRow + i); 
        Console.Write(new string(' ', Console.WindowWidth));
    }

    player.Draw();
    GameLoop.UpdateVisibleElements((player.Position.X, player.Position.Y), LevelData.Elements);
    LevelData.Print(maxX + 1, maxY + 1);

    // Loop will end when game over or player wins
}
while (true);
