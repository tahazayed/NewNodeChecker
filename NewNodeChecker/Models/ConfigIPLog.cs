using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class ConfigIPLog : CommonBusinessEntity
    {
        public int ConfigFileLogId { get; set; }
        [Required]
        public string IP { get; set; }
        public string Key { get; set; }

        [ForeignKey("ConfigFileLogId")]
        public virtual ConfigFileLog ConfigFileLog { get; set; }
    }
}
