using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ENTITYS
{
    public class ExerciseTemplateEntity
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int ExerciseID { get; set; }

        public string TemplateName { get; set; }

        public string DescripMov { get; set; }

        [MaxLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string HandsPosition { get; set; }

        public int RPMMed { get; set; }

        public int RPMFin { get; set; }

        public int ResistancePercentage { get; set; }

        public int DurationMin { get; set; }

        [Column(TypeName = "smallint")]
        public short IsRestingExercise { get; set; }

    }
}
