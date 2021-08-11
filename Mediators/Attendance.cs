using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Mediators
{
    public class Attendance
    {
        public class AttendRequest : IRequest<Result<Unit>>
        {
            public Guid ActivityId { get; set; }
        }

        public class AttendHandler : IRequestHandler<AttendRequest, Result<Unit>>
        {
            private readonly IActivityRepository activityRepository;
            private readonly IUserRepository userRepository;

            public AttendHandler(IActivityRepository activityRepository, IUserRepository userRepository)
            {
                this.activityRepository = activityRepository;
                this.userRepository = userRepository;
            }
            public async Task<Result<Unit>> Handle(AttendRequest request, CancellationToken cancellationToken)
            {
                var result = await new AttendActivity(activityRepository, userRepository).Attend(request.ActivityId);
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to attend activity");
            }
        }

        public class UnattendRequest : IRequest<Result<Unit>>
        {
            public Guid ActivityId { get; set; }
        }

        public class UnattendHandler : IRequestHandler<UnattendRequest, Result<Unit>>
        {
            private readonly IActivityRepository activityRepository;
            private readonly IUserRepository userRepository;

            public UnattendHandler(IActivityRepository activityRepository, IUserRepository userRepository)
            {
                this.activityRepository = activityRepository;
                this.userRepository = userRepository;
            }

            public async Task<Result<Unit>> Handle(UnattendRequest request, CancellationToken cancellationToken)
            {
                var result = await new UnattendActivity(activityRepository, userRepository).Unattend(request.ActivityId);
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to unattend activity");
            }
        }

        
    }
}