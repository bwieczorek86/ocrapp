module OcrPlugin.App.Blazor.E2e.Tests

open OcrPlugin.App.Blazor.E2e.Common
open OcrPlugin.App.Blazor.E2e.Initial.Pages

let underDevelopment () = ()
    //[
        //Initial, TemplateList, ListTemplateTests.run
    //]

let allPages =
    [
        Initial, HomePage, HomePageTests.run
        Initial, TemplateList, ListTemplateTests.run
        Initial, TemplateOcr, TemplateOcrTests.run
        Initial, SettingsPage, SettingsPageTests.run
    ]

let register (e2eType: E2eType option) (page: Page option) (tag: Tag option) =
    let exec predicate =
        allPages
        |> List.filter predicate
        |> List.iter (fun (e, s, func) -> func(s, tag))
        
    match e2eType, page, tag with
        | (_, _, Some UnderDevelopment) -> underDevelopment()
        | (None, None, _) -> exec (fun _ -> true)
        | (None, Some p, _) -> exec (fun (_, p', _) -> p' = p)
        | (Some e, None, _) -> exec (fun (e', _, _) -> e' = e)
        | (Some e, Some p, _) -> exec (fun (e', p', _) -> p' = p && e' = e)
