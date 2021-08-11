using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IPhotoRepository
    {
        Task<bool> Save(Photo photo);
    }
}