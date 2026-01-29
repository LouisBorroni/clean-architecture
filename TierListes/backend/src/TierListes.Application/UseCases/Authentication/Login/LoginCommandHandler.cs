using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Authentication;
using TierListes.Domain.Exceptions;

namespace TierListes.Application.UseCases.Authentication.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result<AuthResponseDto>.Unauthorized("Email ou mot de passe incorrect.");
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result<AuthResponseDto>.Unauthorized("Email ou mot de passe incorrect.");
        }

        user.UpdateLastLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        var response = new AuthResponseDto(
            UserId: user.Id,
            Email: user.Email,
            Username: user.Username,
            Token: token
        );

        return Result<AuthResponseDto>.Success(response, 200);
    }
}
