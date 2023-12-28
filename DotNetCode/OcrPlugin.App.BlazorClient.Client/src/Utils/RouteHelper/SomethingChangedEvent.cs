namespace OcrPlugin.App.BlazorClient.Client.Utils.RouteHelper
{
    public enum SomethingChangedEventType
    {
        Route,
        Search,
        TemplateDeleted,
        ResetSelectedProperties,
        TemplateCordsSet,
    }

    public class SomethingChangedEvent
    {
        public event Func<SomethingChangedEventType, string, Task> Changed;

        public async Task Notify(SomethingChangedEventType eventType, string value)
        {
            if (Changed != null)
            {
                await Changed.Invoke(eventType, value);
            }
        }
    }
}