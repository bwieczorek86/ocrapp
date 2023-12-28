module OcrPlugin.App.Blazor.E2e.Common

open canopy.classic
open Microsoft.FSharp.Reflection
open System.Text.RegularExpressions

//helper funcs
let discriminatedUnionFromString<'a> s =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
        | [|case|] -> FSharpValue.MakeUnion(case,[||]) :?> 'a
        | _ -> failwith <| sprintf "Can't convert %s to DU" s

let optionalDiscriminatedUnionFromString<'a> s =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
        | [|case|] -> FSharpValue.MakeUnion(case,[||]) :?> 'a |> Some
        | _ -> failwith <| sprintf "Can't convert %s to DU" s

let executingDir () =
    let assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location
    System.IO.Path.GetDirectoryName(assemblyLocation)

type Page =
    | HomePage
    | OcrPage
    | TemplateOcr
    | TemplateList
    | TemplateCreate
    | TemplateSetCords
    | TemplateEdit
    | SettingsPage
    | LoginPage

type Tag =
    | Basic
    | UnderDevelopment

type E2eType =
    | Initial
    | Modify

type Arguments =
    {
        Browser: canopy.types.BrowserStartMode
        Page: Page option
        Tag: Tag option
        ReportPath : string
        E2eType : E2eType option
    }
 
let exists selector =
    element selector |> ignore

let doesNotExist selector =
    let node = someElement selector
    match node with
        | Some _ -> failwith "Element was found while it should not be"
        | None -> ()

let matchWithRegexValue (value, regex) =
    let regexMatch = Regex.Match(value, regex)

    match regexMatch.Success with
        | false -> failwith "Value doesn't match regex pattern"
        | _ -> ()

let matchAttributeValueWithRegex (selector, attribute, regex) = 
    let attributeValue = (element selector).GetAttribute(attribute)
    matchWithRegexValue(attributeValue, regex)
