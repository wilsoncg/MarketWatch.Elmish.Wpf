module App

open Elmish.WPF

module ListItem =
  
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
    "Name" |> Binding.oneWay (fun m -> m.Name)
    "Price" |> Binding.oneWay (fun m -> m.Price)
  ] 

module MarketList =

 type Model = {
    Markets: list<ListItem.Market> }
    
 let init() = {
    Markets = Seq.empty<ListItem.Market> |> Seq.toList }

 type Msg =
    | UpdatePrice of int * decimal
    | ParentMsg of int * Msg

 let update msg m =
    match msg with
    | UpdatePrice (id, price) -> { m with Markets = list.Cons ((ListItem.newMarket id price), m.Markets) }

 let bindings model dispatch =
  [
    "Markets" |> Binding.subModelSeq 
        (fun m -> m.Markets)
        (fun cm -> cm.Id)
        (fun () -> ListItem.bindings ())
        ParentMsg
  ]