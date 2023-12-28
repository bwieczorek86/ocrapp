using System.ComponentModel.DataAnnotations.Schema;

namespace OcrPlugin.App.Azure.Storage.Templates
{
    public class PropertyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CordsStartX { get; set; }
        public int CordsStartY { get; set; }
        public int CordsEndX { get; set; }
        public int CordsEndY { get; set; }

        [NotMapped]
        public int Width => CordsEndX - CordsStartX;

        [NotMapped]
        public int Height => CordsEndY - CordsStartY;
    }
}