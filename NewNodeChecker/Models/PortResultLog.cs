using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class PortResultLog : LogBase
    {
        [Required]
        public int PortId { get; set; }
        public bool IsOpened { get; set; }

        [ForeignKey("PortId")]
        public PortInfoDefination PortInfoDefination { get; set; }
    }
}
