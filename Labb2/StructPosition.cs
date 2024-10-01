namespace Labb2
{
    public struct StructPosition
    {
        public int X;
        public int Y;
        public StructPosition(StructPosition position) : this(position.X, position.Y)
        {
            
        }
        public StructPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public double DistanceTo(StructPosition position)
        {
            int deltaX = position.X - X;
            int deltaY = position.Y - Y;

            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
