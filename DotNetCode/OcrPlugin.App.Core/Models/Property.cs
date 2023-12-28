namespace OcrPlugin.App.Core.Models
{
    public class Property
    {
        public string Name { get; set; }
        public int CordsStartX { get; set; }
        public int CordsStartY { get; set; }
        public int CordsEndX { get; set; }
        public int CordsEndY { get; set; }

        public int Width => CordsEndX - CordsStartX;
        public int Height => CordsEndY - CordsStartY;
    }
}