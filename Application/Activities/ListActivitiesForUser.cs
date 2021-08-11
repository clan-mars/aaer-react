using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;

using MediatR;

namespace Application.Activities
{

    public class ListActivitiesForUser
    {
        

        private readonly IActivityRepository activityRepository;

        public ListActivitiesForUser( IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task<List<ActivityDto>> GetList(string username)
        {
            var result = await activityRepository.ListActivitiesForUser(username);

            return result;
        }
    }
}