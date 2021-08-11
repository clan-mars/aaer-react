using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Activities
{
    public class UpdateHostedActivity
    {
        

        private readonly IActivityRepository activityRepository;

        public UpdateHostedActivity(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task<bool> SetCanceled(Guid activityId, bool isCanceled)
        {
            var activity = await activityRepository.GetActivity(activityId);
            if (activity == null) return false;

            activity.IsCancelled = isCanceled;
            await activityRepository.SaveActivity(activity);
            return true;
        }
    }
}