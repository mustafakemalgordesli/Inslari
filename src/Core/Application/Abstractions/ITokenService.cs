using Domain.Entities;

namespace Application.Abstractions;

public interface ITokenService
{
    string CreateToken(User user, int addMonth = 1);
    bool VerifyToken(string token);
}