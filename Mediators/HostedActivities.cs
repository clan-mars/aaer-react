using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Mediators
{
    public class HostedActivities
    {
        public class ToggleActivityCanceledRequest : IRequest<Result<Unit>>
        {
            public Guid ActivityId { get; set; }
            public bool IsCancelled { get; set; }
        }


        public class CancelActivityHandler :  IRequestHandler<ToggleActivityCanceledRequest, Result<Unit>>
        {
            private readonly IActivityRepository activityRepository;

            public CancelActivityHandler(IActivityRepository activityRepository)
            {
                this.activityRepository = activityRepository;
            }

            public async Task<Result<Unit>> Handle(ToggleActivityCanceledRequest request, CancellationToken cancellationToken)
            {
                var result = await new UpdateHostedActivity(activityRepository).SetCanceled(request.ActivityId, request.IsCancelled);
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to update hosted activity, " + request.ActivityId);
            }
        }
    }
}