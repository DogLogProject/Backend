using System.Security.Claims;

namespace ApiProject.JWTManagerRepository
{
    public interface IJWTManagerRepository
    {
        Tokens GenerateToken(string userName, int id);
        Tokens GenerateRefreshToken(string userName, int id);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
