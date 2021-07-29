using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<Activity>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Activity>>
        {
            private readonly DataContext dataContext;
            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;

            }
            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity =  await dataContext.Activities.FindAsync(request.Id);

                return Result<Activity>.Success(activity);
            }
        }
    }
}