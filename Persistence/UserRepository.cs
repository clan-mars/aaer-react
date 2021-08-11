using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
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
            return await context.Users.FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());
        }

        public async Task<AppUser> GetActiveUserWithPhotos()
        {
            return await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());
        }

        public async Task<Domain.Profile> GetProfile(string username) {
            var user = await context.Users.ProjectTo<Domain.Profile>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Username == username);

            return user;
        }

        public string GetActiveUsername()
        {
            return userAccessor.GetUsername();
        }

        public async Task<bool> Save(AppUser user)
        {
            if (user != null) return await context.SaveChangesAsync() > 0;

            return false;
        }
    }
}