module ElmApp
open Elmish.WPF
open Elmish

module TradeItem =
  
 type Market = { 
    Id: int
    Name: string
    Price: decimal
    Open: decimal
    Close: decimal }
    
 let newMarket id price = {
    Id = id
    Name = ""
    Price = price
    Open = 0m
    Close = 0m
 }

 let rec bindings() =
  [
    "Id" |> Binding.oneWay (fun m -> m.Id)
    "Name" |> Binding.oneWay (fun m -> m.Name)
    "Price" |> Binding.oneWay (fun m -> m.Price)
  ] 

module TradesGrid =

 type Model = {
    Markets: list<TradeItem.Market> }
        
 let init() = {
    Markets = Seq.empty<TradeItem.Market> |> Seq.toList }

 let private findMarket (m:Model) id =
  let matchId = (fun (x:TradeItem.Market) -> x.Id = id)
  match m.Markets |> List.exists matchId with
  | true -> Some (m.Markets |> List.find matchId)
  | false -> None

 let updateElement list f = 
  list |> Seq.map f |> Seq.toList
  
 type Msg =
    | UpdatePrice of int * decimal
    | ColumnHeaderClick //of SortOrder
    | ParentMsg of int * Msg

 let update msg model =
    match msg with
    | UpdatePrice (id, price) -> 
         match findMarket model id with
         | Some x -> 
          let market = { x with Price = price; Id = id }
          {model with Markets = updateElement model.Markets (fun m -> if m.Id = id then market else m ) }
         | None -> 
          { model with Markets = model.Markets @ [TradeItem.newMarket id price] }
    | ColumnHeaderClick -> model
    | ParentMsg (id, msg) -> model

 let bindings model dispatch =
  [
    "ColumnHeaderClick" |> Binding.paramCmd (fun p m -> ColumnHeaderClick)
  ]

module Main =
 type Model = { 
    SettingsFlyoutIsOpen: bool
    DarkMode: bool
    TradesGrid: TradesGrid.Model }
 let init() = {
    SettingsFlyoutIsOpen = false
    DarkMode = false
    TradesGrid = TradesGrid.init()
 }
 
 type Msg =
     | SettingsClick of bool
     | TradesGridMsg of int * Msg
 
 let update msg model =
    match msg with
    | SettingsClick state -> { model with SettingsFlyoutIsOpen = state }
    | TradesGridMsg (id, msg) -> model

 let bindings model dispatch =
  [
    "SettingsClick" |> Binding.paramCmd (fun p m -> SettingsClick true)
    "SettingsFlyoutIsOpen" |> Binding.twoWay (fun m -> m.SettingsFlyoutIsOpen) (fun v m -> SettingsClick v)
    "Markets" |> Binding.subModelSeq 
        (fun m -> m.TradesGrid.Markets)
        (fun cm -> cm.Id)
        (fun () -> TradeItem.bindings ())
        TradesGridMsg
  ]
