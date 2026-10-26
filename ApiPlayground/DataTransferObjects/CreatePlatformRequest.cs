using System.ComponentModel.DataAnnotations;

namespace ApiPlayground.DataTransferObjects
{
    public class CreatePlatformRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
    }
}
