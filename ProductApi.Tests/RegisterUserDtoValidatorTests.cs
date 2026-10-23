using FluentValidation.TestHelper;
using ProductApi.Application.Dtos;
using ProductApi.Application.Validators;
using Xunit;

namespace ProductApi.Tests;

public class RegisterUserDtoValidatorTests
{
    private readonly RegisterUserDtoValidator _validator = new();

    [Theory]
    [InlineData(6)]
    [InlineData(20)]
    public void Validate_PasswordAtLimits_HasNoErrors(int length)
    {
        var dto = new RegisterUserDto
        {
            Name = "Test User",
            Username = "testuser",
            Password = new string('a', length)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(5)]
    [InlineData(21)]
    public void Validate_PasswordOutsideLimits_HasPasswordError(int length)
    {
        var dto = new RegisterUserDto
        {
            Name = "Test User",
            Username = "testuser",
            Password = new string('a', length)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_EmptyPassword_ReturnsOnlyRequiredError()
    {
        var dto = new RegisterUserDto
        {
            Name = "Test User",
            Username = "testuser",
            Password = ""
        };

        var result = _validator.TestValidate(dto);

        var error = Assert.Single(result.Errors);

        Assert.Equal(nameof(RegisterUserDto.Password), error.PropertyName);
        Assert.Equal("Şifre boş olamaz.", error.ErrorMessage);
    }
}