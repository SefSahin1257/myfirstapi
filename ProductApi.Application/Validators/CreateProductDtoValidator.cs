using FluentValidation;
using ProductApi.Application.Dtos;

namespace ProductApi.Application.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ürün adı boş olamaz.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0.01m)
            .WithMessage("Ürün fiyatı en az 0,01 olmalıdır.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Geçerli bir kategori seçmelisiniz.");
    }
}