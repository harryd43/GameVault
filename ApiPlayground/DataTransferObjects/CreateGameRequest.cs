using System.ComponentModel.DataAnnotations;

namespace ApiPlayground.DataTransferObjects
{
    public class CreateGameRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Developer { get; set; }
        [Required]
        public DateOnly ReleaseDate { get; set; }   
    }
}
