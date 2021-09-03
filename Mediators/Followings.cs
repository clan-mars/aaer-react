using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Followers;
using Application.Interfaces;
using DTO;
using MediatR;

namespace Mediators
{
    public class Followings
    {
        public class Follow : IRequest<Result<Unit>>
        {
            public string TargetName { get; set; }
        }

        public class FollowHandler : UpdateHandler<Follow>
        {
            private readonly IUserRepository userRepository;
            public FollowHandler(IUserRepository userRepository) : base("Follow")
            {
                this.userRepository = userRepository;
            }

            protected override async Task<bool> DoAction(Follow request)
            {
                return await new FollowUseCase(userRepository).FollowUser(request.TargetName);
            }
        }

        public class Unfollow : IRequest<Result<Unit>>
        {
            public string TargetName { get; set; }
        }

        public class UnfollowHandler : UpdateHandler<Unfollow>
        {
            private readonly IUserRepository userRepository;
            public UnfollowHandler(IUserRepository userRepository) : base("Unfollow")
            {
                this.userRepository = userRepository;
            }

            protected override async Task<bool> DoAction(Unfollow request)
            {
                return await new FollowUseCase(userRepository).UnfollowUser(request.TargetName);
            }
        }

        public class ListFollowers : IRequest<Result<List<ProfileDto>>>
        {
            public string Username { get; set; }
        }

        public class ListFollowersHandler : IRequestHandler<ListFollowers, Result<List<ProfileDto>>>
        {
            private readonly IUserRepository userRepository;

            public ListFollowersHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }
            
            public async Task<Result<List<ProfileDto>>> Handle(ListFollowers request, CancellationToken cancellationToken)
            {
                var followers = await new ListUseCase(userRepository).ListFollowers(request.Username);
                if (followers == null) {
                    return Result<List<ProfileDto>>.Failure("Failed to get followers");
                }

                return Result<List<ProfileDto>>.Success(followers);
            }
        }

        public class ListFollowing : IRequest<Result<List<ProfileDto>>>
        {
            public string Username { get; set; }
        }

        public class ListFollowingHandler : IRequestHandler<ListFollowing, Result<List<ProfileDto>>>
        {
            private readonly IUserRepository userRepository;

            public ListFollowingHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }
            
            public async Task<Result<List<ProfileDto>>> Handle(ListFollowing request, CancellationToken cancellationToken)
            {
                var followers = await new ListUseCase(userRepository).ListFollowing(request.Username);
                if (followers == null) {
                    return Result<List<ProfileDto>>.Failure("Failed to get following");
                }

                return Result<List<ProfileDto>>.Success(followers);
            }
        }

    }
}