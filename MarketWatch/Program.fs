open System
open Elmish
open Elmish.WPF
open App

[<EntryPoint; STAThread>]
let main argv =
  Program.mkSimple App.List.init App.List.update App.List.bindings
  |> Program.withConsoleTrace
  |> Program.runWindowWithConfig
      { ElmConfig.Default with LogConsole = true }
      (WpfControls.AppWindow())