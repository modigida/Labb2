namespace Labb2.Elements
{
    public class Wall : LevelElement
    {
        public Wall(StructPosition position) : base(position)
        {
            Character = '#';
            Color = ConsoleColor.White;
        }
    }
}
