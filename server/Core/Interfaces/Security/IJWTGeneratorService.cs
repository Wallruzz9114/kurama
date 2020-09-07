using Data;

namespace Core.Interfaces.Security
{
    public interface IJWTGeneratorService
    {
        string CreateToken(AppUser appUser);
    }
}