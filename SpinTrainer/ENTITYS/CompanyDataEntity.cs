using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITYS
{
    public class CompanyDataEntity
    {
        [Key]
        public string RIF { get; set; }
        
        [MaxLength(80)]
        [Column(TypeName = "varchar(80)")]
        public string Descrip { get; set; }

        [MaxLength(120)]
        [Column(TypeName = "varchar(120)")]
        public string Direc { get; set; }

        public byte[]? Logo { get; set; }
    }
}
