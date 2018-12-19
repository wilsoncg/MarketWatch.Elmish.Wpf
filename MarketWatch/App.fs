﻿module App

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
    "Id" |> Binding.oneWay (fun m -> m.Id)
    "Name" |> Binding.oneWay (fun m -> m.Name)
    "Price" |> Binding.oneWay (fun m -> m.Price)
  ] 

module MarketList =

 type Model = {
    Markets: list<ListItem.Market> }
        
 let init() = {
    Markets = Seq.empty<ListItem.Market> |> Seq.toList }

 let private findMarket (m:Model) id =
  let matchId = (fun (x:ListItem.Market) -> x.Id = id)
  match m.Markets |> List.exists matchId with
  | true -> Some (m.Markets |> List.find matchId)
  | false -> None

 let updateElement list f = 
  list |> Seq.map f |> Seq.toList

 type Msg =
    | UpdatePrice of int * decimal
    | ParentMsg of int * Msg

 let update msg model =
    match msg with
    | UpdatePrice (id, price) -> 
         match findMarket model id with
         | Some x -> 
          let market = { x with Price = price; Id = id }
          {model with Markets = updateElement model.Markets (fun m -> if m.Id = id then market else m ) }
         | None -> 
          { model with Markets = model.Markets @ [ListItem.newMarket id price] }
    | ParentMsg (id, msg) -> model

 let bindings model dispatch =
  [
    "Markets" |> Binding.subModelSeq 
        (fun m -> m.Markets)
        (fun cm -> cm.Id)
        (fun () -> ListItem.bindings ())
        ParentMsg
  ]