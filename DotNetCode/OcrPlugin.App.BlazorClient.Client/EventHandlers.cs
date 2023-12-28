using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace OcrPlugin.App.BlazorClient.Client;

/// <summary>
/// This is a polyfill until https://github.com/dotnet/aspnetcore/pull/40829
/// gets released in a stable version.
/// </summary>
[EventHandler("onmouseleave", typeof(MouseEventArgs), true, true)]
[EventHandler("onmouseenter", typeof(MouseEventArgs), true, true)]
public static class EventHandlers
{
}