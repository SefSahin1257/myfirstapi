using FluentValidation;
using ProductApi.Application.Dtos;

namespace ProductApi.Application.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ad boş olamaz.");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Kullanıcı adı boş olamaz.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Şifre boş olamaz.")
            .Length(6, 20)
            .WithMessage("Şifre 6 ile 20 karakter arasında olmalıdır");
    }
}