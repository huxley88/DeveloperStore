using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Create;

public record CreateSaleCommand() : IRequest<CreateSaleResult> 
{
   public string SaleNumber { get; set; }
   public Guid CustomerId { get; set; }
   public string CustomerName { get; set; }
   public string BranchId { get; set; }
   public string BranchName {  get; set; }    
   public List<SaleItemModel> Items { get; set; }

    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

