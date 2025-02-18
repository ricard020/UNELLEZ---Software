using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ENTITYS
{
    public class SessionExercisesEntity
    {
        [Key]
        public int ID { get; set; }

        public int SessionID { get; set; }
        
        public int ExerciseID { get; set; }

        public string DescripMov { get; set; }

        [MaxLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string HandsPosition { get; set; }
        
        public int RPMMed { get; set; }

        public int RPMFin { get; set; }

        public int ResistancePercentage { get; set; }

        public int DurationMin { get; set; }

        [JsonIgnore]
        public SessionEntity Session { get; set; }
    }
}
