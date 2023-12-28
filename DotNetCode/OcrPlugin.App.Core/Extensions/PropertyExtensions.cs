using OcrPlugin.App.Core.Models;
using System.Drawing;

namespace OcrPlugin.App.Core.Extensions
{
    public static class PropertyExtensions
    {
        public static Rectangle CreateRectangle(this Property property)
        {
            var rectangle = new Rectangle(
                property.CordsStartX,
                property.CordsStartY,
                property.Width,
                property.Height);

            return rectangle;
        }
    }
}