using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Activities
{
    public class Edit
    {
        private readonly IActivityRepository activityRepository;

        public Edit(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task<bool> PerformEdit(Activity activity) {
            return await activityRepository.SaveActivity(activity);
        }
    }
}