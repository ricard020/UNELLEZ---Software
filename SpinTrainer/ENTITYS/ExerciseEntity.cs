using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITYS
{
    public class ExerciseEntity
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(120)]
        [Column(TypeName = "varchar(120)")]
        public string Descrip { get; set; }
       
        public int RPMMin { get; set; }

        public int RPMMax { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string HandsPositions { get; set; }

        [NotMapped]
        public string ExerciseImageRoute
        {
            get
            {
                switch (Descrip)
                {
                    case "Plano Sentado":
                        return "iconoplanosentado.png";
                    case "Plano de Pie / Correr":
                        return "iconoplanodepiecorrer.png";
                    case "Saltos":
                        return "iconosaltos.png";
                    case "Escalada Sentado":
                        return "iconoescaladasentado.png";
                    case "Escalada de Pie":
                        return "iconoescaladadepie.png";
                    case "Correr en Montaña":
                        return "iconocorrerenmontana.png";
                    case "Saltos en Montaña":
                        return "iconosaltosenmontana.png";
                    case "Sprints en Plano":
                        return "iconosprintsenplano.png";
                    case "Sprints en Montaña":
                        return "iconosprintsenmontana.png";
                    default:
                        return "";
                }
            }

        }
    }
}
