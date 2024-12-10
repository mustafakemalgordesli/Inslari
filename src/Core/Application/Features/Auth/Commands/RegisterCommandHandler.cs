using Application.Abstractions;
using Application.Responses;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Result;
using Domain.UnitOfWork;

namespace Application.Features.Auth.Commands;

public class RegisterCommand : ITransactionalCommand<Result<AuthResponse>>
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class RegisterCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordHasher passwordHasher) : ITransactionalCommandHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (!IsValidUsername(request.Username)) return Result<AuthResponse>.Failure(UserErrors.InvalidUsername);

        var existUser = await userRepository.GetAsync(x => x.Email == request.Email || x.Username == request.Email);

        if (existUser is not null) return Result<AuthResponse>.Failure(UserErrors.UserExist);

        var user = new User()
        {
            Username = request.Username,
            Email = request.Email,
            Password = passwordHasher.HashPassword(request.Password)
        };

        user = userRepository.Add(user);

        user?.UserRegistered();
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

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

    private static readonly List<string> BannedUsernames = new List<string>
    {
        "admin", "signup", "signin", "login", "register"
    };

    private bool IsValidUsername(string username)
    {
        if (BannedUsernames.Contains(username.ToLower()))
        {
            return false;
        }

        if (!username.All(ch => char.IsLower(ch) || char.IsDigit(ch)))
        {
            return false;
        }

        return true;
    }
}