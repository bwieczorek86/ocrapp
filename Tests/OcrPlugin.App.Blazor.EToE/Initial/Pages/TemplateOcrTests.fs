module OcrPlugin.App.Blazor.E2e.Initial.Pages.TemplateOcrTests

open canopy.classic
open canopy.runner.classic
open OcrPlugin.App.Blazor.E2e
open OcrPlugin.App.Blazor.E2e.Common

let basic (baseUrl: string) =
    "There is a title on the page" &&& fun _ ->
        sleep 1
        contains "Strona OCR - E2e" (read "h1")

let all(baseUrl: string) =
    basic(baseUrl)

let run (page: Page, tag: Tag option) =
    context (sprintf "Template ocr page")
    let baseUrl = PagesPaths.getUrlAndSupply page "E2e";

    once (
        fun _ ->
            url baseUrl
    )
    match tag with
        | None -> all(baseUrl)
        | _ -> ()
