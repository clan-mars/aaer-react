using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{

    public class ListActivitiesForUser
    {
        public class Query : IRequest<Result<List<ActivityDto>>> { public string Username { get; set; } }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                ListActivitiesForUser listActivitiesForUser = new ListActivitiesForUser(context, mapper);
                var result = await listActivitiesForUser.GetList(request.Username);
                return Result<List<ActivityDto>>.Success(result);
            }
        }

        private readonly DataContext context;
        private readonly IMapper mapper;
        public ListActivitiesForUser(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<List<ActivityDto>> GetList(string username)
        {
            var result = await context.Activities
                .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var a in result)
            {
                a.IsGoing = a.Attendees.Any(p => p.Username == username);
                a.IsHost = a.HostUsername == username;
                a.Host = a.Attendees.Single(ac => ac.Username == a.HostUsername);
                a.Date = a.Date == null ? DateTime.Now : a.Date;
            }

            return result;
        }
    }
}