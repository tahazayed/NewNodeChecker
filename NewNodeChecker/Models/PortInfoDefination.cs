using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class PortInfoDefination : DefinationBase
    {
        
        [Required]
        public string Ip4Address { get; set; }

        [Required]
        public int PortNo { get; set; }

        public virtual List<PortResultLog> PortResults { get; set; } 
    }
}
