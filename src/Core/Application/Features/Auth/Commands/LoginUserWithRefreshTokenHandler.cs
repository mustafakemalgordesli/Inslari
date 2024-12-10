using Application.Abstractions;
using Application.Responses;
using Domain.Repositories;
using Domain.Result;
using Domain.UnitOfWork;

namespace Application.Features.Auth.Commands;

public sealed record LoginUserWithRefreshToken(string RefreshToken) : ICommand<Result<AuthResponse>>;

public class LoginUserWithRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenService tokenService) : ICommandHandler<LoginUserWithRefreshToken, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginUserWithRefreshToken request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenIncludeUser(request.RefreshToken);

        if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)     
            throw new ApplicationException("Token.RefreshTokenExpired");

        string accessToken = tokenService.CreateToken(refreshToken.User);

        refreshToken.Token = tokenService.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddMonths(1);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken.Token });
    }
}
