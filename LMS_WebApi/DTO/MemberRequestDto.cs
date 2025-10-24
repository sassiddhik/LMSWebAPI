using LMS_WebApi.Enum;
using System.ComponentModel.DataAnnotations;

namespace LMS_WebApi.DTO
{
    public class MemberRequestDto
    {
        [StringLength(100, MinimumLength = 3)]
        [Required]
        public required string UserName { get; set; }

        [StringLength(80, MinimumLength = 3)]
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [StringLength(255, MinimumLength = 8)]
        [Required]
        public required string Password { get; set; }

        public string? ProfilePicture { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
    }
}
