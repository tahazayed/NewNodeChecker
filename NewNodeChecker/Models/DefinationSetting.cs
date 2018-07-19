using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class DefinationSetting : CommonBusinessEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
