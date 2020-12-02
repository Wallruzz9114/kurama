using System.Threading.Tasks;
using Data.ViewModels;

namespace Core.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUsername();
        Task<ProfileViewModel> GetProfile(string username);
    }
}