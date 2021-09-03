using System.Collections.Generic;
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
        Task<UserFollowing> GetUserFollowing(string observer, string target);
        Task<bool> RemoveUserFollowing(UserFollowing following);
        Task<bool> Save(AppUser user);
        Task<AppUser> GetUser(string username);
        Task<bool> AddUserFollowing(UserFollowing following);
        Task<List<ProfileDto>> GetFollowingProfiles(string userName);
        Task<List<ProfileDto>> GetFollowerProfiles(string userName);
    }
}