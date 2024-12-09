using Application.Abstractions;
using Application.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Result;
using MediatR;

namespace Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<AuthResponse>>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}


public class LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(x => x.Email == request.Username || x.Username == request.Username);

        if (user is null) return Result<AuthResponse>.Failure(UserErrors.UserNotFound);

        if (!passwordHasher.VerifyPassword(user.Password, request.Password)) return Result<AuthResponse>.Failure(UserErrors.PasswordWrong);

        var response = new AuthResponse { AccessToken = tokenService.CreateToken(user), RefreshToken = tokenService.CreateToken(user, addMonth: 12) };

        return Result<AuthResponse>.Success(response);
    }
}