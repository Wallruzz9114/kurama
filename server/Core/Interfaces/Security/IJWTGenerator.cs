using Data;

namespace Core.Interfaces.Security
{
    public interface IJWTGenerator
    {
        string CreateToken(AppUser appUser);
    }
}