using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Mediators
{
    public class Activities
    {
        public class Create : IRequest<Result<Unit>> { public Activity Activity { get; set; } }
        public class CreateHandler : UpdateHandler<Create>
        {
            private readonly IActivityRepository activityRepository;
            private readonly IUserRepository userRepository;

            public CreateHandler(IActivityRepository activityRepository, IUserRepository userRepository) : base("create")
            {
                this.activityRepository = activityRepository;
                this.userRepository = userRepository;
            }

            protected override async Task<bool> DoAction(Create request)
            {
                return await new Application.Activities.Create(activityRepository, userRepository).PerformCreate(request.Activity);
            }
        }

        public class Delete : IRequest<Result<Unit>> { public Guid Id { get; set; } }

        public class DeleteHandler : UpdateHandler<Delete>
        {
            private readonly IActivityRepository activityRepository;
            public DeleteHandler(IActivityRepository activityRepository) : base("delete")
            {
                this.activityRepository = activityRepository;
            }

            protected override async Task<bool> DoAction(Delete request)
            {
                return await new Application.Activities.Delete(activityRepository).PerformDelete(request.Id);
            }
        }

        public class Edit : IRequest<Result<Unit>> { public ActivityDto Activity { get; set; } }

        public class EditHandler : UpdateHandler<Edit>
        {
            private readonly IActivityRepository activityRepository;

            public EditHandler(IActivityRepository activityRepository) : base("update")
            {
                this.activityRepository = activityRepository;
            }

            protected override async Task<bool> DoAction(Edit request)
            {
                return await new Application.Activities.Edit(activityRepository).PerformEdit(request.Activity);
            }
        }

        public class ListUserActivities : IRequest<Result<List<ActivityDto>>> { public string Username { get; set; } }

        public class ListUserActivitiesHandler : IRequestHandler<ListUserActivities, Result<List<ActivityDto>>>
        {
            private readonly IActivityRepository activityRepository;

            public ListUserActivitiesHandler(IActivityRepository activityRepository)
            {
                this.activityRepository = activityRepository;
            }

            public async Task<Result<List<ActivityDto>>> Handle(ListUserActivities request, CancellationToken cancellationToken)
            {
                ListActivitiesForUser listActivitiesForUser = new ListActivitiesForUser(activityRepository);
                var result = await listActivitiesForUser.GetList(request.Username);
                return Result<List<ActivityDto>>.Success(result);
            }
        }

        public class GetDetails : IRequest<Result<ActivityDto>> { public Guid Id { get; set; } }

        public class Handler : IRequestHandler<GetDetails, Result<ActivityDto>>
        {
            private readonly IActivityRepository activityRepository;
            private readonly IUserRepository userRepository;

            public Handler(IActivityRepository activityRepository, IUserRepository userRepository)
            {
                this.activityRepository = activityRepository;
                this.userRepository = userRepository;
            }
            public async Task<Result<ActivityDto>> Handle(GetDetails request, CancellationToken cancellationToken)
            {
                var activity = await new Application.Activities.Details(activityRepository, userRepository).Get(request.Id);
                if (activity == null)
                {
                    return Result<ActivityDto>.Failure("Could not find activity");
                }

                return Result<ActivityDto>.Success(activity);
            }
        }

        public class ActivityList : IRequest<Result<PagedList<ActivityDto>>>
        {
            public PagingParams PagingParams { get; set; }
        }

        public class ListHandler : IRequestHandler<ActivityList, Result<PagedList<ActivityDto>>>
        {
            private readonly IActivityRepository activityRepository;

            public ListHandler(IActivityRepository activityRepository)
            {
                this.activityRepository = activityRepository;
            }
            public async Task<Result<PagedList<ActivityDto>>> Handle(ActivityList request, CancellationToken cancellationToken)
            {
                var result = await new Application.Activities.List(activityRepository).GetList(request.PagingParams);

                return Result<PagedList<ActivityDto>>.Success(result);
            }
        }


    }
}