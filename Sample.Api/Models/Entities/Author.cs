using System.ComponentModel.DataAnnotations;

namespace Sample.Api.Models.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(250, ErrorMessage = "Name length can't be more than 250.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Name length can't be more than 250.")]
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; } 
    }
}
