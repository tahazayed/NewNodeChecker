using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public  class ConfigConnectionStringLog : CommonBusinessEntity
    {
        public int ConfigFileLogId { get; set; }
        public int SqlConnectionDefinationId { get; set; }
        [Required]  
        public string ConnectionSting { get; set; }

        [ForeignKey("ConfigFileLogId")]
        public virtual ConfigFileLog ConfigFileLog { get; set; }
        [ForeignKey("SqlConnectionDefinationId")]
        public virtual SqlConnectionDefination SqlConnectionDefination { get; set; }
    }
}
