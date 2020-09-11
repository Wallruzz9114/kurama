using System.Threading.Tasks;
using Data;

namespace Core.Interfaces.Security
{
    public interface IAppUserService
    {
        string GetCurrentAppUserUsername();
        Task<ProfileViewModel> GetProfile(string username);
    }
}