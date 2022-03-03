using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using DTO;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IUserAccessor userAccessor;
        private readonly IMapper mapper;

        public UserRepository(DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            this.userAccessor = userAccessor;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<AppUser> GetActiveUser()
        {
            return await GetUser(userAccessor.GetUsername());
        }

        public async Task<AppUser> GetUser(string username)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetActiveUserWithPhotos()
        {
            return await context.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());
        }

        public async Task<ProfileDto> GetProfile(string username)
        {
            var user = await context.Users.ProjectTo<ProfileDto>(mapper.ConfigurationProvider,
                CreateUserConfig())
            .SingleOrDefaultAsync(x => x.Username == username);

            return user;
        }

        public async Task<bool> FollowUser(string targetName)
        {
            var observer = await GetActiveUser();

            var target = await GetUser(targetName);
            if (target == null) return false;

            var following = await context.UserFollowings.FindAsync(observer.Id, target.Id);
            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                context.UserFollowings.Add(following);
                return await context.SaveChangesAsync() > 0;
            }

            return true;
        }

        public async Task<UserFollowing> GetUserFollowing(string observer, string target)
        {
            return await context.UserFollowings.FindAsync(observer, target);
        }

        public async Task<bool> RemoveUserFollowing(UserFollowing following)
        {
            context.UserFollowings.Remove(following);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<ProfileDto>> GetFollowingProfiles(string userName)
        {
            return await context.UserFollowings
            .Where(x => x.Observer.UserName == userName)
            .Select(u => u.Target).ProjectTo<ProfileDto>(mapper.ConfigurationProvider, CreateUserConfig())
            .ToListAsync();
        }

        public async Task<List<ProfileDto>> GetFollowerProfiles(string userName)
        {
            return await context.UserFollowings
            .Where(x => x.Target.UserName == userName)
            .Select(u => u.Observer).ProjectTo<ProfileDto>(mapper.ConfigurationProvider, CreateUserConfig())
            .ToListAsync();
        }

        private object CreateUserConfig() 
        => new { currentUsername = userAccessor.GetUsername() };

        public string GetActiveUsername() 
        => userAccessor.GetUsername();

        public async Task<bool> Save(AppUser user)
        {
            if (user != null) return await context.SaveChangesAsync() > 0;

            return false;
        }

        public async Task<bool> AddUserFollowing(UserFollowing following)
        {
            context.UserFollowings.Add(following);
            return await context.SaveChangesAsync() > 0;
        }
    }
}