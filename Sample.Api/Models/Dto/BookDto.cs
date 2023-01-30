using System.ComponentModel.DataAnnotations;

namespace Sample.Api.Models.Dto
{
    public record BookDto
    {
        public BookDto()
        {
            
        }
        public int Id { get; set; }	
        [Required]
        [StringLength(10, ErrorMessage = "Name length can't be more than 10.")]
        public string ISBN { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Name length can't be more than 250.")]
        public string Name { get; set; }
        [Required]
        public  int AuthorId { get; set; }
    }
}