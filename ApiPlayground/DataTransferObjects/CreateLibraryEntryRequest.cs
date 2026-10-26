using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiPlayground.DataTransferObjects
{
    public class CreateLibraryEntryRequest
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Range(1, int.MaxValue)]
        public int GameId { get; set; }
        [Range(1, int.MaxValue)]
        public int PlatformId { get; set; }
        [Required]
        [EnumDataType(typeof(CompletionStatus))]
        public CompletionStatus? CompletionStatus { get; set; }

    }
}

