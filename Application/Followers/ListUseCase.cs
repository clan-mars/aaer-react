using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using DTO;

namespace Application.Followers
{
    public class ListUseCase
    {
        private readonly IUserRepository userRepository;

        public ListUseCase(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<List<ProfileDto>> ListFollowing(string username) {
            return await userRepository.GetFollowingProfiles(username);
        }

        public async Task<List<ProfileDto>> ListFollowers(string username)
        {
            return await userRepository.GetFollowerProfiles(username);
        }
    }
}