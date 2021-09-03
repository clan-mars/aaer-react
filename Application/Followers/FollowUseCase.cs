using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Followers
{
    public class FollowUseCase
    {
        private readonly IUserRepository userRepository;

        public FollowUseCase(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> FollowUser(string targetName)
        {
            var user = await userRepository.GetActiveUser();
            var target = await userRepository.GetUser(targetName);
            var following = await userRepository.GetUserFollowing(user.Id, target.Id);
            if (following == null)
            {
                following = new Domain.UserFollowing
                {
                    Observer = user,
                    Target = target
                };

                return await userRepository.AddUserFollowing(following);
            }

            return true;
        }

        public async Task<bool> UnfollowUser(string targetName)
        {
            var user = await userRepository.GetActiveUser();
            var target = await userRepository.GetUser(targetName);
            var following = await userRepository.GetUserFollowing(user.Id, target.Id);
            if (following != null)
            {
                return await userRepository.RemoveUserFollowing(following);
            }

            return true;
        }

    }
}