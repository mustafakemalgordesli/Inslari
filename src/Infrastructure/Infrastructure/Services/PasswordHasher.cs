using System.Security.Cryptography;
using Application.Abstractions;

namespace Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations: Iterations, hashAlgorithm: Algorithm, outputLength: HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        string[] parts = hashedPassword.Split('-');
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations: Iterations, hashAlgorithm: Algorithm, outputLength: HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
