open System

namespace ValidationOrderLine

open ManualPurchaseOrder
open Products
open System
open ManualPurchaseOrder

type ValidationOrderLine(orderLine: ManualPurchaseOrderLine, ?productProperties: ProductProperties) =
    member private this.priceGreaterThanZero =
        let validation = fun () ->
            let ruleName = "priceGreaterthanZero"
            match this._price with
                | Some p -> match p with
                                | p when p > 0m -> ValidationResult.Successful ruleName
                                | _ -> ValidationResult.Unsuccessful ruleName
                | None -> ValidationResult.Unsuccessful ruleName
        validation

    member private this.deliveryDateNotInThePast: Func<ValidationResult> =
        let validation =
            fun() ->
                let ruleName = "deliveryDateNotInThePast"
                match this._requestedDeliveryDate with
                    | Some d -> match d with
                                    | d when d.Date >= DateTime.Today.Date -> ValidationResult.Successful ruleName
                                    | _ -> ValidationResult.Unsuccessful ruleName
                    | None -> ValidationResult.Unsuccessful ruleName
        validation

    member private this.quantityGreaterThanZero =
        let validation = fun() ->
            let ruleName = "quantityGreaterThanZero"
            match this._quantity with
                | Some q -> match q with
                                | q when q > 0 -> ValidationResult.Successful ruleName
                                | _ -> ValidationResult.Unsuccessful ruleName
                | None -> ValidationResult.Unsuccessful ruleName
        validation

    member this.MustHaveDeliveryDate: Unit =
        let validation = fun() ->
            let ruleName = "MustHaveDeliveryDate"
            match this._requestedDeliveryDate with
                | Some _ -> ValidationResult.Successful ruleName
                | None -> ValidationResult.Unsuccessful ruleName
        this.RequestedDeliveryDateValidation.AddValidator validation
        this.RequestedDeliveryDateValidation.AddValidator this.deliveryDateNotInThePast


    member this.RequestedDeliveryDateValidation:ValidationField<Option<DateTime>> = ValidationField this._requestedDeliveryDate
    
    member private this._price = orderLine.Price
    member private this._quantity = orderLine.Quantity
    member private this._requestedDeliveryDate: Option<DateTime> = orderLine.RequestedDeliveryDate
    member this.LineNumber = orderLine.LineNumber
    


