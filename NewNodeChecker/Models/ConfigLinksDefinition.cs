using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class ConfigLinksDefinition : DefinationBase
    {

        [Required]
        public string Links { get; set; }

    }
}
