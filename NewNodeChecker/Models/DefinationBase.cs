using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class DefinationBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [DefaultValue("true")]
        public bool IsEnabled { get; set; }

        [Required]
        public int DefinationSettingId { get; set; }

        [ForeignKey("DefinationSettingId")]
        public DefinationSetting DefinationSetting { get; set; }

        public DefinationBase()
        {
            IsEnabled = true;
        }
    }
}
