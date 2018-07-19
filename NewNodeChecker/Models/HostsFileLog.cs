using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class HostsFileLog : LogBase
    {
       [Required]
        public string FileContent { get; set; }
      
    }
}