module OcrPlugin.App.Blazor.E2e.PagesPaths
open OcrPlugin.App.Blazor.E2e.Common

let getPath = function
    | HomePage -> "/"
    | OcrPage -> "ocr"
    | TemplateOcr -> "ocr/"
    | TemplateList -> "templates/list"
    | TemplateCreate -> "template-create"
    | TemplateSetCords -> "template/"
    | TemplateEdit -> "templates/edit/"
    | SettingsPage -> "settings"
    | LoginPage -> "Login"

let getDomain () = "https://localhost:5003/"

let getUrl (page: Page) = getDomain() + getPath(page)

let getUrlAndSupply (page: Page) (url: string) = getDomain() + getPath(page) + url