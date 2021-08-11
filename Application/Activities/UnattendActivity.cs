using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Activities
{
    public class UnattendActivity
    {
        
        private readonly IActivityRepository activityRepository;
        private readonly IUserRepository userRepository;

        public UnattendActivity(IActivityRepository activityRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.activityRepository = activityRepository;
        }

        public async Task<bool> Unattend(Guid activityId)
        {
            var activity = await activityRepository.GetRealActivity(activityId);

            if (activity == null)
            {
                return false;
            }

            var attendance = activity.Attendees.FirstOrDefault(ac => ac.AppUser.UserName == userRepository.GetActiveUsername());

            if (attendance == null) return true;

            activity.Attendees.Remove(attendance);
            return await activityRepository.PlainSave();
        }

    }
}