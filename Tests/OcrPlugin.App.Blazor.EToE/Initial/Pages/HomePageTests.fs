module OcrPlugin.App.Blazor.E2e.Initial.Pages.HomePageTests

open canopy.classic
open canopy.runner.classic
open OcrPlugin.App.Blazor.E2e
open OcrPlugin.App.Blazor.E2e.Common

let basic (baseUrl: string) =
    "There is a title on the page" &&& fun _ ->
        contains "Pushen ocr!" (read "h1")

let all(baseUrl: string) =
    basic(baseUrl)

let run (page: Page, tag: Tag option) =
    context (sprintf "Home page")
    let baseUrl = PagesPaths.getUrl page;

    once (
        fun _ ->
            url baseUrl
    )
    match tag with
        | None -> all(baseUrl)
        | _ -> ()
