using Domain;
using Persistence;

namespace Application.Activities
{
    public interface ICleanCreate
    {
        void Process(Activity input);
    }

    public class CleanCreate
    {
        private readonly DataContext dataContext;
        public CleanCreate(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void Process(Activity input)
        {
            dataContext.Activities.Add(input);
        }
    }
}