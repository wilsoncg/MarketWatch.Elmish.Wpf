module MainWindow

open Elmish.WPF
open FsXaml
open MahApps.Metro.Controls

type MainWindow = MetroWindow 

type Model = {
}

type Msg = 
    | Nothing

let init() = {}

let update msg model = 
    match msg with
    | Nothing -> model

let bindings model dispatch =
 [
    "ClosingTabItemHandler" |> Binding.tw
 ]