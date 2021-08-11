using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Mediators
{
    public class Profiles
    {
        public class Details : IRequest<Result<Profile>> { public string Username { get; set; }}

        public class DetailsHandler : IRequestHandler<Details, Result<Profile>> {
            private readonly IUserRepository userRepository;

            public DetailsHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }
            
            public async Task<Result<Profile>> Handle(Details request, CancellationToken cancellationToken)
            {
                var profile = await userRepository.GetProfile(request.Username);
                return Result<Profile>.Success(profile);
            }
        }

    }
}