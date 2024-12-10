using Domain.Entities;

namespace Application.Abstractions;

public interface ITokenService
{
    string CreateToken(User user);
    string GenerateRefreshToken();
    bool VerifyToken(string token);
}