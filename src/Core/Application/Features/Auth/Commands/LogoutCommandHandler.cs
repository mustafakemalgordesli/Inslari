using System.Security.Claims;
using Application.Abstractions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Result;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands;

public sealed record LogoutCommand(string Token) : ICommand<Result> { }

public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : ICommandHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        if (userId == null) return Result.Failure(AuthErrors.AuthError);

        var refreshToken = await refreshTokenRepository.GetAsync(r => r.Token == request.Token && r.UserId == userId);

        if (refreshToken == null) return Result.Failure(AuthErrors.AuthError);

        refreshToken.IsDeleted = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    private Guid? GetCurrentUserId()
    {
        return Guid.TryParse(
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out Guid parsed)
            ? parsed : null;
    }
}
