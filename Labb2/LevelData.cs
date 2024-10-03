using Labb2.Elements;

namespace Labb2
{
    public class LevelData
    {
        private static List<LevelElement> _elements = new List<LevelElement>();
        public static List<LevelElement> Elements
        {
            get { return _elements; }
        }
        public LevelData(Player player)
        {
            _elements.Add(player);
        }
        public List<LevelElement> Load(string textfile)
        {
            string[] lines = System.IO.File.ReadAllLines(textfile);
            foreach (var (line, y) in lines.Select((line, index) => (line, index)))
            {
                LoadElementsFromLine(line, y);
            }
            return Elements;
        }
        private void LoadElementsFromLine(string line, int y)
        {
            foreach (var (character, x) in line.Select((c, index) => (c, index)))
            {
                var element = CreateElement(character, x, y);
                if (element != null)
                {
                    _elements.Add(element);
                }
            }
        }
        private LevelElement? CreateElement(char character, int x, int y)
        {
            return character switch
            {
                '#' => new Wall(new StructPosition(x, y)),
                'r' => new Rat(new StructPosition(x, y)),
                's' => new Snake(new StructPosition(x, y)),
                _ => null
            };
        }
        public static void Print(int width, int height)
        {
            char[,] maze = InitializeMaze(width, height);
            PlaceElementsInMaze(maze);
            DrawMaze(maze);
        }
        private static char[,] InitializeMaze(int width, int height)
        {
            char[,] maze = new char[height, width];
        
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j] = ' ';
                }
            }
            return maze;
        }
        private static void PlaceElementsInMaze(char[,] maze)
        {
            foreach (var element in Elements)
            {
                char character = GetElementCharacter(element);
                maze[element.Position.Y, element.Position.X] = character;
            }
        }
        private static char GetElementCharacter(LevelElement element)
        {
            return element switch
            {
                Wall => '#',
                Rat => 'r',
                Snake => 's',
                _ => ' ' 
            };
        }
        private static void DrawMaze(char[,] maze)
        {
            int height = maze.GetLength(0);
            int width = maze.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                DrawMazeRow(i, width);
                Console.WriteLine(); 
            }
        }
        private static void DrawMazeRow(int row, int width)
        {
            for (int j = 0; j < width; j++)
            {
                DrawElementAtPosition(j, row);
            }
        }
        private static void DrawElementAtPosition(int x, int y)
        {
            var element = Elements.FirstOrDefault(e => e.Position.X == x && e.Position.Y == y);

            if (element != null)
            {
                DrawElement(element);
            }
        }
        private static void DrawElement(LevelElement element)
        {
            switch (element)
            {
                case Wall wall:
                    wall.Draw();
                    break;
                case Rat rat:
                    rat.Draw();
                    break;
                case Snake snake:
                    snake.Draw();
                    break;
            }
        }
    }
}
