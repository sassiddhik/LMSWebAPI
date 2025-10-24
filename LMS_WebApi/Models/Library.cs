using System.ComponentModel.DataAnnotations;

namespace LMS_WebApi.Models
{
    public class Library
    {
        [Key]
        public Guid LibraryID { get; set; }

        [Required, MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        public ICollection<Book> Books { get; set; }

    }
}
