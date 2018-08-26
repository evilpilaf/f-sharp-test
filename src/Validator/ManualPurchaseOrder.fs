namespace ManualPurchaseOrder

open System

type ManualPurchaseOrderLine = {
    LineNumber: int;
    ProductId: Option<int>;
    Ean: Option<string>;
    Mpn: Option<string>;
    Quantity: Option<int>;
    Price: Option<decimal>;
    RequestedDeliveryDate: Option<DateTime>;
}

type ValidationResult private (ruleName: string, wasRuleSuccesful: bool) =
    static member Successful ruleName = new ValidationResult(ruleName, true)
    static member Unsuccessful ruleName = new ValidationResult(ruleName, false)
    member this.RuleName = ruleName
    member this.WasRuleSuccessful = wasRuleSuccesful

type ValidationField<'T> private(value: 'T, validatorFunctions: List<Func<ValidationResult>>) =
    new value = ValidationField<'T>(value, List.Empty)
    member this.ValidatorFuncs = validatorFunctions;
    member this.Value = value
    member this.AddValidator(validationFunc) = 
        new ValidationField<'T>(this.Value, List.append this.ValidatorFuncs [validationFunc])
    member this.ValidationResults = 
        seq {
            for validation in this.ValidatorFuncs do
                let result = validation.Invoke()
                yield result
        }
    member this.IsValid = Seq.forall (fun (x: ValidationResult) -> x.WasRuleSuccessful) this.ValidationResults
