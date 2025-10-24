using LMS_WebApi.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace LMS_WebApi.Models
{
    public class Member
    {
        [Key]
        public Guid Id { get; set; }
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
        public bool? IsBanned { get; set; } = false;
        public bool? IsLocked { get; set; } = false;
        [Required]
        public UserRole UserRole { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}

