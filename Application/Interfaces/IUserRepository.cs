using System.Threading.Tasks;
using Domain;
using DTO;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetActiveUser();
        string GetActiveUsername();
        Task<AppUser> GetActiveUserWithPhotos();
        Task<ProfileDto> GetProfile(string username);
        Task<bool> Save(AppUser user);
    }
}