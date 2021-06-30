using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext dataContext;
            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;

            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(request.Id);
                dataContext.Remove(activity);
                await dataContext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}