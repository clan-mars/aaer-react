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
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly IActivityRepository activityRepository;

            public Handler(IActivityRepository activityRepository)
            {
                this.activityRepository = activityRepository;
            }
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await activityRepository.ListActivities();

                return Result<List<ActivityDto>>.Success(result);
            }
        }
    }
}