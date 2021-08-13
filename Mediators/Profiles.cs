using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using DTO;
using MediatR;

namespace Mediators
{
    public class Profiles
    {
        public class Details : IRequest<Result<ProfileDto>> { public string Username { get; set; } }

        public class DetailsHandler : IRequestHandler<Details, Result<ProfileDto>>
        {
            private readonly IUserRepository userRepository;

            public DetailsHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result<ProfileDto>> Handle(Details request, CancellationToken cancellationToken)
            {
                var profile = await userRepository.GetProfile(request.Username);
                return Result<ProfileDto>.Success(profile);
            }
        }

        public class Update : IRequest<Result<Unit>>
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class ProfileUpdateHandler : UpdateHandler<Update>
        {
            private IUserRepository userRepository;

            public ProfileUpdateHandler(IUserRepository userRepository) : base("Update")
            {
                this.userRepository = userRepository;
            }

            protected override async Task<bool> DoAction(Update request)
            {
                return await new Application.Profiles.Update(userRepository).DoUpdate(request.DisplayName, request.Bio);
            }
        }

    }
}