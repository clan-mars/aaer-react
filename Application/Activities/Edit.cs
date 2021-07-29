using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext dataContext;
            private readonly IMapper mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                this.mapper = mapper;
                this.dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(request.Activity.Id);

                if (activity == null) return null;

                mapper.Map(request.Activity, activity);
                var result = await dataContext.SaveChangesAsync() > 0;
                
                if (result) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update activity");
            }
        }

    }
}