using Models;

namespace Core.Interfaces
{
    public interface IJWTGenerator
    {
        string CreateToken(AppUser appUser);
    }
}