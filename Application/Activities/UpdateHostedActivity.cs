using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class UpdateHostedActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ActivityId { get; set; }
            public bool IsCancelled { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await new UpdateHostedActivity(context).SetCanceled(request.ActivityId, request.IsCancelled);
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to update hosted activity, " + request.ActivityId);
            }
        }

        private DataContext context;

        public UpdateHostedActivity(DataContext context)
        {
            this.context = context;
        }

        public async Task<bool> SetCanceled(Guid activityId, bool isCanceled)
        {
            var activity = await context.Activities.FindAsync(activityId);
            if (activity == null) return false;

            activity.IsCancelled = isCanceled;
            await context.SaveChangesAsync();
            return true;
        }
    }
}