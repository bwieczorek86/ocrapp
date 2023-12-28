module OcrPlugin.App.Blazor.E2e.Program

open OcrPlugin.App.Blazor.E2e
open canopy
open canopy.classic
open canopy.runner.classic
open reporters

[<EntryPoint>]
let main argv =
  let args = Args.parse argv

  configuration.chromeDir <- Common.executingDir()
  configuration.failureScreenshotsEnabled <- false
  configuration.pageTimeout <- 5.0
  configuration.elementTimeout <- 1.0
  configuration.compareTimeout <- 1.0
  configuration.reporter <- new JUnitReporter("testsResults/canopy/EndToEnd.xml") :> types.IReporter

  start args.Browser
  Authorization.login()
  Tests.register args.E2eType args.Page args.Tag
  run ()
  quit ()

  canopy.runner.classic.failedCount
