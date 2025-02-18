using ENTITYS;
using Microsoft.EntityFrameworkCore;
using UTILITIES.CryptographyDataUtility;

namespace REPOSITORY.DBContext
{
    public class SQLiteDBContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<SessionEntity> Sessions { get; set; }
        public virtual DbSet<SessionExercisesEntity> SessionExercises { get; set; }
        public virtual DbSet<ExerciseEntity> Exercises { get; set; }
        public virtual DbSet<ExerciseTemplateEntity> ExerciseTemplate { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "spintrainer.db3");

                //optionsBuilder.
                    //UseSqlite("File Name=database.db");

                optionsBuilder
                    .UseSqlite($"Filename={dbPath}");

                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExerciseEntity>().HasData(
                new ExerciseEntity
                {
                    ID = 1,
                    Descrip = "Plano Sentado",
                    RPMMin = 80,
                    RPMMax = 110,
                    HandsPositions = "1,2,2.5"
                },
                new ExerciseEntity
                {
                    ID = 2,
                    Descrip = "Plano de Pie / Correr",
                    RPMMin = 80,
                    RPMMax = 110,
                    HandsPositions = "2,2.5"
                },
                new ExerciseEntity
                {
                    ID = 3,
                    Descrip = "Saltos",
                    RPMMin = 80,
                    RPMMax = 110,
                    HandsPositions = "2,2.5"
                },
                new ExerciseEntity
                {
                    ID = 4,
                    Descrip = "Escalada Sentado",
                    RPMMin = 60,
                    RPMMax = 80,
                    HandsPositions = "2,2.5"
                },
                new ExerciseEntity
                {
                    ID = 5,
                    Descrip = "Escalada de Pie",
                    RPMMin = 60,
                    RPMMax = 80,
                    HandsPositions = "3"
                },
                new ExerciseEntity
                {
                    ID = 6,
                    Descrip = "Correr en Montaña",
                    RPMMin = 60,
                    RPMMax = 80,
                    HandsPositions = "2,2.5"
                },
                new ExerciseEntity
                {
                    ID = 7,
                    Descrip = "Saltos en Montaña",
                    RPMMin = 60,
                    RPMMax = 80,
                    HandsPositions = "2,2.5,3"
                },
                new ExerciseEntity
                {
                    ID = 8,
                    Descrip = "Sprints en Plano",
                    RPMMin = 80,
                    RPMMax = 110,
                    HandsPositions = "2,2.5,3"
                },
                new ExerciseEntity
                {
                    ID = 9,
                    Descrip = "Sprints en Montaña",
                    RPMMin = 80,
                    RPMMax = 110,
                    HandsPositions = "2,2.5,3"
                }
            );

            modelBuilder.Entity<SessionEntity>()
                .HasMany(i => i.SessionExercises)
                .WithOne(p => p.Session)
                .HasForeignKey(i => i.SessionID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
