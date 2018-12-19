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
    //o.Subscribe(
    //    fun (timestamp, value) -> 
    //     MarketList.update(MarketList.Msg.UpdatePrice (1, decimal value)) |> ignore) |> ignore
    //o.Subscribe(fun f -> 
    //    match f with 
    //    | (timestamp, value) -> MarketList.update(MarketList.Msg.UpdatePrice m)) |> ignore

    //let timer initial =
    //    let sub dispatch = dispatch o.Next
    //        //window.setInterval(fun _ -> 
    //        //    dispatch (Tick DateTime.Now)
    //        //    , 1000) |> ignore
    //    Cmd.ofSub sub
    
    //let timerTick dispatch =
    //  let timer = new System.Timers.Timer(1000.)
    //  timer.Elapsed.Add (fun _ -> 
    //    let updateMsg =
    //     let u = o.Next() |> Seq.toList
    //     let timestamp, value = u.Head
    //     (1, decimal value)
    //    dispatch <| MarketList.Msg.UpdatePrice updateMsg
    //  )
    //  timer.Start()

    let timerTick dispatch =
        o.Subscribe(
            fun (timestamp, value) -> 
            dispatch <| MarketList.Msg.UpdatePrice (1, decimal value)) |> ignore        

    [<EntryPoint; STAThread>]
    let main argv =
      let window = MainWindow()
      Program.mkSimple MarketList.init MarketList.update MarketList.bindings 
      |> Program.withConsoleTrace
      |> Program.withSubscription (fun _ -> Cmd.ofSub timerTick)
      |> Program.runWindowWithConfig
          { ElmConfig.Default with LogConsole = true }
          (window)