using FluentValidation;
using Repositories.Products;

namespace Services.Products.Update;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        //name validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün ismi gereklidir.")
            .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır.");

        //price validation
        RuleFor(y => y.Price)
            .GreaterThan(0).WithMessage("Ürün fiyatı 0 dan büyük olmalıdır.");

        //stock inclusive between validation
        RuleFor(z => z.Stock)
            .InclusiveBetween(1, 100).WithMessage("stok adedi 1 ile 100 arasında olmalıdır.");
    }
}
