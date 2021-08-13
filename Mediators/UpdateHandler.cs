using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;

namespace Mediators
{
        public abstract class UpdateHandler<T> : IRequestHandler<T, Result<Unit>> where T : IRequest<Result<Unit>>
        {
            private readonly string action;

            protected UpdateHandler(string action)
            {
                this.action = action;
            }

            public async Task<Result<Unit>> Handle(T request, CancellationToken cancellationToken)
            {
                var result = await DoAction(request);

                if (result)
                {
                    return Result<Unit>.Success(Unit.Value);
                }

                return Result<Unit>.Failure($"Failed to {action} activity");
            }

            protected abstract Task<bool> DoAction(T request);
        }
}