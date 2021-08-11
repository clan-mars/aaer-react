using System.Threading.Tasks;
using Application.Interfaces;
using Domain;


namespace Application.Activities
{
    public class Create
    {
        private readonly IActivityRepository activityRepository;
        private readonly IUserRepository userRepository;

        public Create(IActivityRepository activityRepository, IUserRepository userRepository)
        {
            this.activityRepository = activityRepository;
            this.userRepository = userRepository;
        }

        public async Task<bool> PerformCreate(Activity activity)
        {
            var user = await userRepository.GetActiveUser();

            var attendee = new ActivityAttendee
            {
                AppUser = user,
                Activity = activity,
                IsHost = true
            };

            activity.Attendees.Add(attendee);

            return await activityRepository.SaveActivity(activity);
        }
    }
}