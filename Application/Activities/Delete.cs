using System;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Activities
{
    public class Delete
    {
        private readonly IActivityRepository activityRepository;

        public Delete(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task<bool> PerformDelete(Guid id) {
            return await activityRepository.DeleteActivity(id);
        }
    }
}