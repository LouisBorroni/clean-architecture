using FluentValidation;

namespace TierListes.Application.UseCases.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("L'email est requis")
            .EmailAddress()
            .WithMessage("L'email n'est pas valide")
            .MaximumLength(255)
            .WithMessage("L'email ne peut pas dépasser 255 caractères");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Le nom d'utilisateur est requis")
            .MinimumLength(3)
            .WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères")
            .MaximumLength(50)
            .WithMessage("Le nom d'utilisateur ne peut pas dépasser 50 caractères");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Le mot de passe est requis")
            .MinimumLength(6)
            .WithMessage("Le mot de passe doit contenir au moins 6 caractères")
            .MaximumLength(100)
            .WithMessage("Le mot de passe ne peut pas dépasser 100 caractères");
    }
}
