using System.Collections.Generic;
using Application.Activities;

namespace DTO
{
    public class ProfileDto
    {
        public string Username { get; set; }
        public string DisplayName {get;set;}
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public ICollection<PhotoDto> Photos {get;set;}
        public ICollection<AttendeeDto> Activities {get;set;}
    }
}