using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetActiveUser();
        string GetActiveUsername();
        Task<AppUser> GetActiveUserWithPhotos();
        Task<Profile> GetProfile(string username);
        Task<bool> Save(AppUser user);
    }
}