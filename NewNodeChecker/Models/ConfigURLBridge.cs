using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class ConfigURLBridge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ConfigId { get; set; }
        public int URLId { get; set; }



    }
}
