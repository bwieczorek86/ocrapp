namespace OcrPlugin.App.BlazorClient.Client.Utils
{
    public static class Medias
    {
        // { /* smartphones, iPhone, portrait 480x320 phones */ }
        public static string SmartPhones = "320px";

        // { /* portrait e-readers (Nook/Kindle), smaller tablets @ 600 or @ 640 wide. */ }
        public static string EReaders = "481px";

        // { /* portrait tablets, portrait iPad, landscape e-readers, landscape 800x480 or 854x480 phones */ }
        public static string PortraitTablets = "641px";

        // { /* tablet, landscape iPad, lo-res laptops ands desktops */ }
        public static string Tablets = "961px";

        // { /* big landscape tablets, laptops, and desktops */ }
        public static string Desktops = "1025px";

        // { /* hi-res laptops and desktops */ }
        public static string HiResDesktops = "1921px";
    }
}