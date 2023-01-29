
using System.ComponentModel.DataAnnotations;

namespace Sample.Models.Dto
{
    public record AuthorDto
    {
        public AuthorDto()
        {
            
        }
        public int Id { get; set; }	
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}