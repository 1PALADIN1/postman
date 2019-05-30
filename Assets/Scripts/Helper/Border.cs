namespace Core.Helper
{
    public struct Border
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public Border(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY; 
        }
    }
}
