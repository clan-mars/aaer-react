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
    public class AttendActivity
    {

        public class Command : IRequest<Result<Unit>>
        {
            public string Username { get; set; }
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
                var result = await new AttendActivity(context, userAccessor).Attend(request.ActivityId);
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to attend activity");
            }
        }

        private readonly DataContext context;
        private readonly IUserAccessor userAccessor;

        public AttendActivity(DataContext context, IUserAccessor userAccessor)
        {
            this.userAccessor = userAccessor;
            this.context = context;
        }

        public async Task<bool> Attend(Guid activityId)
        {
            var activity = await context.Activities
                .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                .FirstOrDefaultAsync(x => x.Id == activityId);

            if (activity == null)
            {
                return false;
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

            if (user == null) return false;

            if (activity.Attendees.Any(ac => ac.AppUser.UserName == user.UserName))
            {
                return true;
            }

            var attendance = new Domain.ActivityAttendee
            {
                AppUser = user,
                Activity = activity,
                IsHost = false
            };

            activity.Attendees.Add(attendance);

            return await context.SaveChangesAsync() > 0;
        }
    }
}