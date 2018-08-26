namespace Validator

type SupplierType =
      SupplierOnly = 1
    | SupplierAcceptingRepairs = 2
    | RepairCentre = 3

type Supplier(supplierId: int, supplierType:SupplierType) =
    member this.IsElegibleForOrdering(): bool = 
        match this.SupplierType with 
            | SupplierType.RepairCentre -> false
            | _ -> true
    member this.SupplierId = supplierId
    member this.SupplierType = supplierType

module Validator =
    let validateSupplier (supplier: Option<Supplier>): bool =
        match supplier with
            | Some s -> s.IsElegibleForOrdering()
            | None -> false
