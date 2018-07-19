using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class ConfigLinksLog :  LogBase
    {
        public int ConfigLinksDefinitionId { get; set; }
        [Required]
        public string Status { get; set; }

        [ForeignKey("ConfigLinksDefinitionId")]
        public virtual ConfigLinksDefinition ConfigLinksDefinition { get; set; }
    }
}
