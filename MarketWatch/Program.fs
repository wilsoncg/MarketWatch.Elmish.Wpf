open System
open System.Reactive
open System.Reactive.Linq
open Elmish
open Elmish.WPF
open ElmApp
open Views

module Window =
    type App = FsXaml.XAML<"App.xaml">
    type MainWindow = FsXaml.XAML<"MainWindow.xaml">

    let o = 
        Observable
         .Timer(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
         .Timestamp()
         .Select(fun f -> f, Math.Sin(float f.Value))

    let timerTick dispatch =
        o.Subscribe(
            fun (timestamp, price) -> 
            let rounded = Math.Round(price, 2)
            let id = Random().Next(100)
            dispatch <| TradesGrid.Msg.UpdatePrice (id, decimal rounded)) |> ignore        

    [<EntryPoint; STAThread>]
    let main argv =
      App() |> ignore
      SettingsFlyout() |> ignore
      Program.mkSimple Main.init Main.update Main.bindings 
      |> Program.withConsoleTrace
      |> Program.runWindowWithConfig
          { ElmConfig.Default with LogConsole = true }
          (MainWindow())