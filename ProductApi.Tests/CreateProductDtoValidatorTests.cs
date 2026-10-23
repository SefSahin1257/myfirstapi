using FluentValidation.TestHelper;
using ProductApi.Application.Dtos;
using ProductApi.Application.Validators;
using Xunit;

namespace ProductApi.Tests;

public class CreateProductDtoValidatorTests
{
    private readonly CreateProductDtoValidator _validator = new();

    [Fact]
    public void Validate_ValidProduct_HasNoErrors()
    {
        var dto = new CreateProductDto
        {
            Name = "Klavye",
            Price = 1500m,
            CategoryId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyName_HasNameError(string name)
    {
        var dto = new CreateProductDto
        {
            Name = name,
            Price = 1500m,
            CategoryId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Validate_InvalidPrice_HasPriceError(int price)
    {
        var dto = new CreateProductDto
        {
            Name = "Klavye",
            Price = price,
            CategoryId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidCategoryId_HasCategoryError(int categoryId)
    {
        var dto = new CreateProductDto
        {
            Name = "Klavye",
            Price = 1500m,
            CategoryId = categoryId
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Fact]
    public void Validate_MinimumPrice_HasNoPriceError()
    {
        var dto = new CreateProductDto
        {
            Name = "Kalem",
            Price = 0.01m,
            CategoryId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }
}