using Labb2;

string startMessage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProgramDescription", "DescriptionAndRules.txt");
try
{
    string fileContent = File.ReadAllText(startMessage);
    Console.WriteLine(fileContent);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
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
try
{
    leveldata.Load(textfile);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
}

var player = new Player(new StructPosition(1, 4));

int maxX = LevelData.Elements.Max(element => element.Position.X);
int maxY = LevelData.Elements.Max(element => element.Position.Y);
do
{
    var game = new GameLoop(player);
    int startRow = 3; 
    int numRowsToClear = 20;
    for (int i = 0; i < numRowsToClear; i++)
    {
        Console.SetCursorPosition(0, startRow + i); 
        Console.Write(new string(' ', Console.WindowWidth));
    }
    
    GameLoop.UpdateVisibleElements((player.Position.X, player.Position.Y), LevelData.Elements);
    LevelData.Print(maxX + 1, maxY + 1);
    player.Draw();
}
while (true);
