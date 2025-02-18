using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITYS
{
    public class SessionEntity
    {
        [Key]
        public int ID { get; set; }
        public int TrainerID { get; set; }

        [MaxLength(120)]
        [Column(TypeName = "varchar(120)")]
        public string Descrip { get; set; }

        public DateTime DateC { get; set; }

        public DateTime DateI { get; set; }

        public int Duration { get; set; }

        //public bool IsShared { get; set; }
        
        //public int TrainerIDWhoSharesIt { get; set; }

        public ICollection<SessionExercisesEntity> SessionExercises { get; set; }
    }
}
