using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext dataContext;
            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;

            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(request.Id);

                dataContext.Remove(activity);
                var result = await dataContext.SaveChangesAsync() > 0;

                if (result) {
                    return Result<Unit>.Success(Unit.Value);
                }

                return Result<Unit>.Failure("Failed to delete activity");
            }
        }
    }
}