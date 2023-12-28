using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace OcrPlugin.App.BlazorClient.Client.Utils;

public static class JsRuntimeExtensions
{
    public static bool IsMiddleClickClicked(this MouseEventArgs e)
    {
        const int middleButtonClickId = 1;
        var isMiddleClickClicked = e.Button == middleButtonClickId;

        return isMiddleClickClicked;
    }
}

public class BetterNavigationManager
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;

    public BetterNavigationManager(
        IJSRuntime jsRuntime,
        NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
    }

    public async Task NavigateTo(string url, bool newTab)
    {
        if (newTab)
        {
            await _jsRuntime.InvokeAsync<object>("open", url, "_blank");
        }
        else
        {
            _navigationManager.NavigateTo(url);
        }
    }
}