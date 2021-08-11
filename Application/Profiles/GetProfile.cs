using Application.Interfaces;

namespace Application.Profiles
{
    public class GetProfile
    {
        private readonly IUserRepository userRepository;
        public GetProfile(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

        }
    }
}