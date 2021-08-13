using System.Collections.Generic;

namespace DTO
{
    public class ProfileDto
    {
        public string Username { get; set; }
        public string DisplayName {get;set;}
        public string Bio { get; set; }
        public string Image { get; set; }
        public ICollection<PhotoDto> Photos {get;set;}
    }
}