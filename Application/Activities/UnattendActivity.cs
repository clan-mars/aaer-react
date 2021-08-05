using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UnattendActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this.context = context;
                this.userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await new UnattendActivity(context, userAccessor).Unattend(request.ActivityId);
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to unattend activity");
            }
        }

        private readonly DataContext context;
        private readonly IUserAccessor userAccessor;

        public UnattendActivity(DataContext context, IUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        public async Task<bool> Unattend(Guid activityId)
        {
            var activity = await context.Activities
                .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                .FirstOrDefaultAsync(x => x.Id == activityId);

            if (activity == null)
            {
                return false;
            }

            var attendance = activity.Attendees.FirstOrDefault(ac => ac.AppUser.UserName == userAccessor.GetUsername());

            if (attendance == null) return true;

            activity.Attendees.Remove(attendance);

            return await context.SaveChangesAsync() > 0;
        }

    }
}