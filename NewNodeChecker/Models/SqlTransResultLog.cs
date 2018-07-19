using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class SqlTransResultLog : LogBase
    {
        [Required] public int SqlConnectionDefinationId { get; set; }
        public int RowsCount { get; set; }

        public bool Status { get; set; }

        [ForeignKey("SqlConnectionDefinationId")]
        public virtual SqlConnectionDefination SqlConnectionDefination { get; set; }
    }
}
