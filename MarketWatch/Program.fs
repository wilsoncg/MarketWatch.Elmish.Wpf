open System
open Elmish
open Elmish.WPF
open App

module Window =
    type MainWindow = FsXaml.XAML<"MainWindow.xaml">

    [<EntryPoint; STAThread>]
    let main argv =
      let window = MainWindow()
      Program.mkSimple MarketList.init MarketList.update MarketList.bindings 
      |> Program.withConsoleTrace
      |> Program.runWindowWithConfig
          { ElmConfig.Default with LogConsole = true }
          (window)