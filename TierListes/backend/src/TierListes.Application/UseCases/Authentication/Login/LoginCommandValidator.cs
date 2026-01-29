using FluentValidation;

namespace TierListes.Application.UseCases.Authentication.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("L'email est requis")
            .EmailAddress()
            .WithMessage("L'email n'est pas valide");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Le mot de passe est requis");
    }
}
