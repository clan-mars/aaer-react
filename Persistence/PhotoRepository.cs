using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext dataContext;
        private readonly IUserAccessor userAccessor;

        public PhotoRepository(DataContext dataContext, IUserAccessor userAccessor)
        {
            this.userAccessor = userAccessor;
            this.dataContext = dataContext;
        }

        public async Task<bool> Save(Photo photo)
        {
            var existing = await dataContext.Photos.FindAsync(photo.Id);
            if (existing != null) {
                dataContext.Photos.Remove(existing);
                dataContext.Photos.Add(photo);
            } else {
                dataContext.Photos.Add(photo);
            }
            
            return await dataContext.SaveChangesAsync() > 0;
        }
    }
}