using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Activities
{
    public class List
    {
        private readonly IActivityRepository activityRepository;

        public List(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task<PagedList<ActivityDto>> GetList(PagingParams pagingParams) {
            return await activityRepository.ListActivities(pagingParams);
        }
    }
}