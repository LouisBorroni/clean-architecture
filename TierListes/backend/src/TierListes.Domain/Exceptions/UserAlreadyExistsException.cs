namespace TierListes.Domain.Exceptions;

public class UserAlreadyExistsException : DomainException
{
    public UserAlreadyExistsException(string email)
        : base($"Un utilisateur avec l'email '{email}' existe déjà.") { }
}
