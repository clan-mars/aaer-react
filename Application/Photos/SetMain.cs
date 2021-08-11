using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Photos
{
    public class SetMain
    {
        private IUserRepository userRepository;

        public SetMain(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> PerformSetMain(string photoId)
        {
            var user = await userRepository.GetActiveUserWithPhotos();
            if (user == null)
            {
                return false;
            }

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null)
            {
                currentMain.IsMain = false;
            }

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
            {
                return false;
            }

            photo.IsMain = true;

            return await userRepository.Save(user);
        }
    }
}