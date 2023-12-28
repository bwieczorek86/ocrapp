module OcrPlugin.App.Blazor.E2e.Authorization

open canopy.classic
open canopy.runner.classic
open Common

let login () =
    url (PagesPaths.getUrl LoginPage)
    "#form_login" << "ugabuga@ugabuga.ugabuga"
    "#form_password" << "ugabuga@ugabuga.ugabuga"
    click "#form_submit"

let logout () =
    click "#logout"

let setupAuthorization () =
    once (fun _ ->
        login()
    )
    lastly (fun _ ->
        logout())