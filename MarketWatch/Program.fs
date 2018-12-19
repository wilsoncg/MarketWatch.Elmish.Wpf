open System
open System.Reactive
open System.Reactive.Linq
open Elmish
open Elmish.WPF
open App

module Window =
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
            dispatch <| MarketList.Msg.UpdatePrice (1, decimal rounded)) |> ignore        

    [<EntryPoint; STAThread>]
    let main argv =
      let window = MainWindow()
      Program.mkSimple MarketList.init MarketList.update MarketList.bindings 
      |> Program.withConsoleTrace
      |> Program.withSubscription (fun _ -> Cmd.ofSub timerTick)
      |> Program.runWindowWithConfig
          { ElmConfig.Default with LogConsole = true }
          (window)