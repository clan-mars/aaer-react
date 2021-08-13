using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Profiles
{
    public class Update
    {
        private readonly IUserRepository userRepository;

        public Update(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> DoUpdate(string displayName, string bio) {
            if (string.IsNullOrEmpty(displayName))  return false;

            var user = await userRepository.GetActiveUser();
            user.DisplayName = displayName;
            user.Bio = bio;
            return await userRepository.Save(user);
        }
    }
}