using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Authentication;
using TierListes.Domain.Entities;
using TierListes.Domain.Exceptions;

namespace TierListes.Application.UseCases.Authentication.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
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
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return Result<AuthResponseDto>.Conflict(
                $"Un utilisateur avec l'email '{request.Email}' existe déjà."
            );
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(
            email: request.Email,
            username: request.Username,
            passwordHash: passwordHash
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        var response = new AuthResponseDto(
            UserId: user.Id,
            Email: user.Email,
            Username: user.Username,
            Token: token
        );

        return Result<AuthResponseDto>.Success(response, 201);
    }
}
