using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Photos
{
    public class Add
    {
        private readonly IPhotoRepository photoRepository;
        private readonly IUserRepository userRepository;
        private readonly IPhotoAccessor photoAccessor;

        public Add(IPhotoRepository photoRepository, IUserRepository userRepository, IPhotoAccessor photoAccessor)
        {
            this.photoRepository = photoRepository;
            this.userRepository = userRepository;
            this.photoAccessor = photoAccessor;
        }
        public async Task<Photo> PerformAdd(IFormFile file)
        {
            var user = await userRepository.GetActiveUserWithPhotos();
            if (user == null) return null;
            var result = await photoAccessor.AddPhoto(file);

            var photo = new Photo
            {
                Url = result.Url,
                Id = result.PublicId
            };

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            await userRepository.Save(user);

            return photo;
        }
    }
}