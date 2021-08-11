using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;

namespace Application.Activities
{
    public class AttendRequest : UseCaseRequest<bool> {}
    public class AttendActivity
    {
        private readonly IActivityRepository activityRepository;
        private readonly IUserRepository userRepository;

        public AttendActivity(IActivityRepository activityRepository, IUserRepository userRepository)
        {
            this.activityRepository = activityRepository;
            this.userRepository = userRepository;
        }

        public async Task<bool> Attend(Guid activityId)
        {
            var activity = await activityRepository.GetRealActivity(activityId);

            if (activity == null)
            {
                return false;
            }

            var user = await userRepository.GetActiveUser();

            if (activity.Attendees.Any(ac => ac.AppUser.UserName == user.UserName))
            {
                return true;
            }

            var attendance = new ActivityAttendee
            {
                AppUser = user,
                Activity = activity,
                IsHost = false
            };

            activity.Attendees.Add(attendance);

            return await activityRepository.PlainSave();
        }
    }
}