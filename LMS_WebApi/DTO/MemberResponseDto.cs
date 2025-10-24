using LMS_WebApi.Enum;

namespace LMS_WebApi.DTO
{
    public class MemberResponseDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime DateJoined { get; set; }
        public bool isBanned { get; set; } = false;
        public bool isLocked { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}
