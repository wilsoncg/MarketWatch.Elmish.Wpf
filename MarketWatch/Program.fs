open System
open System.Reactive
open System.Reactive.Linq
open Elmish
open Elmish.WPF
open FsXaml
open MahApps.Metro.Controls
open App

module Window =
    type App = XAML<"App.xaml">
    type MainWindow = MainWindow.MainWindow

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
            dispatch <| MarketList.Msg.UpdatePrice (id, decimal rounded)) |> ignore        

    [<EntryPoint; STAThread>]
    let main argv =
      let window = MainWindow()
      App().MainWindow <- window      
      Program.mkSimple MainWindow.init MainWindow.update MainWindow.bindings 
      |> Program.withConsoleTrace
      //|> Program.withSubscription (fun _ -> Cmd.ofSub timerTick)
      |> Program.runWindowWithConfig
          { ElmConfig.Default with LogConsole = true }
          (window)