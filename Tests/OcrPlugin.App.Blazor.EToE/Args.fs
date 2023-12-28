module OcrPlugin.App.Blazor.E2e.Args

open Argu
open OcrPlugin.App.Blazor.E2e.Common
open Microsoft.FSharp.Reflection
open System

type private CliArguments =
  | Browser of string
  | Page of string
  | Tag of string
  | ReportPath of string
  | E2eType of string
  with
    interface IArgParserTemplate with
      member s.Usage =
        match s with
            | Browser _ -> "Specify a browser (ChromeHeadless | Chrome)"
            | Page _ -> (sprintf "Specify a page (%s)" (String.Join(", ", (FSharpType.GetUnionCases typeof<Page>))))
            | Tag _ -> (sprintf "Specify a test type (%s)" (String.Join(", ", (FSharpType.GetUnionCases typeof<Tag>))))
            | ReportPath _ -> "Specify a report path"
            | E2eType _ -> (sprintf "Specify a test module (%s)" (String.Join(", ", (FSharpType.GetUnionCases typeof<E2eType>))))

let parse cliArguments =
    let parser = ArgumentParser.Create<CliArguments>()
    let results = parser.Parse(cliArguments)
    {
        Browser = defaultArg (results.TryPostProcessResult (Browser, discriminatedUnionFromString)) canopy.types.BrowserStartMode.ChromeHeadless
        Page = defaultArg (results.TryPostProcessResult (Page, optionalDiscriminatedUnionFromString)) None
        Tag = defaultArg (results.TryPostProcessResult (Tag, optionalDiscriminatedUnionFromString)) None
        ReportPath = results.GetResult(ReportPath, defaultValue = "testsResults/canopy/EndToEnd.xml") 
        E2eType = defaultArg (results.TryPostProcessResult (E2eType, optionalDiscriminatedUnionFromString)) None
    }