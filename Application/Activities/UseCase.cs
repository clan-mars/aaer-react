using System.Threading.Tasks;

namespace Application.Activities
{
    public interface UseCaseRequest <T> {}
    public interface UseCase<T>
    {
        Task<T> Perform(UseCaseRequest<T> request);
    }
}