using Application.Abstractions;
using Application.Responses;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Result;
using Domain.UnitOfWork;
using MediatR;

namespace Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<AuthResponse>>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}


public class LoginCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordHasher passwordHasher) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserByUsernameOrEmailAsync(request.Username);

        if (user is null) return Result<AuthResponse>.Failure(UserErrors.UserNotFound);

        if (!passwordHasher.VerifyPassword(user.Password, request.Password)) return Result<AuthResponse>.Failure(UserErrors.PasswordWrong);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = tokenService.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddMonths(1)
        };

        refreshToken = refreshTokenRepository.Add(refreshToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse { AccessToken = tokenService.CreateToken(user), RefreshToken = refreshToken.Token };

        return Result<AuthResponse>.Success(response);
    }

    private async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
    {
        if (usernameOrEmail.Contains("@"))
            return await userRepository.GetAsync(u => u.Email == usernameOrEmail);

        return await userRepository.GetAsync(u => u.Username == usernameOrEmail);
    }
}