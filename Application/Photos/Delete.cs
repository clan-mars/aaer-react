using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Photos
{
    public class Delete
    {
        private readonly IPhotoAccessor photoAccessor;
        private readonly IUserRepository userRepository;

        public Delete(IPhotoAccessor photoAccessor, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.photoAccessor = photoAccessor;
        }

        public async Task<bool> PerformDelete(string photoId)
        {
            var user = await userRepository.GetActiveUserWithPhotos();
            if (user == null) return false;

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null)
            {
                return true;
            }

            if (photo.IsMain)
            {
                return false;
            }

            var result = await photoAccessor.DeletePhoto(photoId);
            user.Photos.Remove(photo);
            await userRepository.Save(user);
            return true;
        }
    }
}