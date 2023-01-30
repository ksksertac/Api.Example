
using System.ComponentModel.DataAnnotations;

namespace Sample.Api.Models.Dto
{
    public record AuthorDto
    {
        public AuthorDto()
        {
            
        }
        public int Id { get; set; }	

        [Required]
        [StringLength(250, ErrorMessage = "FirstName length can't be more than 250.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "LastName length can't be more than 250.")]
        public string LastName { get; set; }
    }
}