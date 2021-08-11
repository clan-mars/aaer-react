using System;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Activities
{
    public class Details
    {
        private readonly IActivityRepository activityRepository;
        private readonly IUserRepository userRepository;

        public Details(IActivityRepository activityRepository, IUserRepository userRepository)
        {
            this.activityRepository = activityRepository;
            this.userRepository = userRepository;
        }

        public async Task<ActivityDto> Get(Guid id)
        {
            string activeUsername = userRepository.GetActiveUsername();
            var activity = await activityRepository.GetActivity(id, activeUsername);

            return activity;
        }
    }
}