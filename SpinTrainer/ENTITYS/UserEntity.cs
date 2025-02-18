using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITYS
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string Username { get; set; }

        [MaxLength(300)]
        [Column(TypeName = "varchar(300)")]
        public string Descrip { get; set; }

        [Column(TypeName = ("varbinary(MAX)"))]
        public string Password { get; set; }

        [Column(TypeName = ("varbinary(MAX)"))]
        public string PIN { get; set; }

        [MaxLength(120)]
        [Column(TypeName = "varchar(120)")]
        public string Email { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string NumberPhone { get; set; }

        public DateTime DateC { get; set; }
        public DateTime DateR { get; set; }
        public DateTime DateV { get; set; }

        [Column(TypeName = "smallint")]
        public short UserType { get; set; }
    }
}