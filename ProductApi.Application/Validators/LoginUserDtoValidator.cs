using FluentValidation;
using ProductApi.Application.Dtos;

namespace ProductApi.Application.Validators;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Kullanıcı adı boş olamaz.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Şifre boş olamaz");
    }
}