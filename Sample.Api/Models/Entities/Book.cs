using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Api.Models.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50.")]
        public string ISBN { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Name length can't be more than 250.")]
        public string Name { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
        [Required]
        public virtual int AuthorId { get; set; }
    }
}
