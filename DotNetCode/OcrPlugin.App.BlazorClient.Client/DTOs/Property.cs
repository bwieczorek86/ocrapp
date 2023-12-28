namespace OcrPlugin.App.BlazorClient.Client.DTOs
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

        public bool IsNotEmpty()
        {
            return CordsStartX != 0 && CordsStartY != 0 && CordsEndX != 0 && CordsEndY != 0;
        }
    }
}