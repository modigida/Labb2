using Labb2.Elements;

namespace Labb2
{
    public class LevelData
    {
        private List<LevelElement> elements = new List<LevelElement>();
        public List<LevelElement> Elements
        {
            get { return elements; }
        }
        public List<LevelElement> Load(string textfile)
        {
            string[] lines = System.IO.File.ReadAllLines(textfile);

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];

                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        elements.Add(new Wall(new StructPosition(x, y)));
                    }
                    else if (line[x] == 'r')
                    {
                        elements.Add(new Rat(new StructPosition(x, y)));
                    }
                    else if (line[x] == 's')
                    {
                        elements.Add(new Snake(new StructPosition(x, y)));
                    }
                }
            }
            return Elements;
        }
        public static void Print(int width, int height, List<LevelElement> elements)
        {
            char[,] maze = InitializeMaze(width, height);
            PlaceElementsInMaze(maze, elements);
            DrawMaze(maze, elements);
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
        private static void PlaceElementsInMaze(char[,] maze, List<LevelElement> elements)
        {
            foreach (var element in elements)
            {
                if (element is Wall)
                {
                    maze[element.Position.Y, element.Position.X] = '#';
                    LevelElement.Walls.Add(element);
                }
                else if (element is Rat)
                {
                    maze[element.Position.Y, element.Position.X] = 'r';
                    LevelElement.Enemies.Add(element);
                }
                else if (element is Snake)
                {
                    maze[element.Position.Y, element.Position.X] = 's';
                    LevelElement.Enemies.Add(element);
                }
            }
        }
        private static void DrawMaze(char[,] maze, List<LevelElement> elements)
        {
            int height = maze.GetLength(0);
            int width = maze.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    foreach (var element in elements)
                    {
                        if (element.Position.X == j && element.Position.Y == i)
                        {
                            if (element is Wall)
                            {
                                ((Wall)element).Draw();
                            }
                            else if (element is Rat)
                            {
                                ((Rat)element).Draw();
                            }
                            else if (element is Snake)
                            {
                                ((Snake)element).Draw();
                            }
                            break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
