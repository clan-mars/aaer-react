using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext dataContext;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                this.dataContext = dataContext;
                this.mapper = mapper;
                this.userAccessor = userAccessor;
            }
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity =  await dataContext.Activities
                .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

                var activeUsername = userAccessor.GetUsername();
                activity.IsHost = activity.HostUsername == activeUsername;
                activity.IsGoing = activity.Attendees.Any(ac => ac.Username == activeUsername);

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}