using ENTITYS;
using UTILITIES.CryptographyDataUtility;
using Microsoft.EntityFrameworkCore;

namespace REPOSITORY.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public virtual DbSet<CompanyDataEntity> CompanyData { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<ExerciseEntity> Exercises { get; set; }
        public virtual DbSet<SessionExercisesEntity> SessionExercises { get; set; }
        public virtual DbSet<SessionEntity> Sessions { get; set; }
        public virtual DbSet<ExerciseTemplateEntity> ExerciseTemplate { get; set; }

        private readonly ICryptographyDataUtility _cryptographyDataUtility;

        public ApplicationDBContext()
        {
            _cryptographyDataUtility = new CryptographyDataUtility();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString;

                try
                {
                    //Obtiene la ruta del ejecutable
                    string executablePath = AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = "Application.cfg";
                    string filePath = System.IO.Path.Combine(executablePath, fileName);

                    //Lee y desencripta la cadena de conexión
                    string connectionStringEncriptada = File.ReadAllText(filePath);
                    connectionString = _cryptographyDataUtility.Decrypt(connectionStringEncriptada);
                }
                catch (Exception ex)
                {
                    //Manejo de errores para conexión vacía o cualquier excepción
                    connectionString = "";
                    throw;
                }

                ////Poner la conexion manual cuando se vayan a crear las migraciones
                //connectionString = "Data Source=.;" +
                //                   "Initial Catalog=SpinTrainer;" +
                //                   "TrustServerCertificate=True;" +
                //                   "Persist Security Info=True;" +
                //                   "User Id=sa;" +
                //                   "Password=123456 ;" +
                //                   "Connect Timeout=60;";

                // Configura SQL Server usando la cadena de conexión desencriptada
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity 
                {
                    Id = 1,
                    Username = "SU",
                    Descrip = "Super Usuario",
                    Password = (_cryptographyDataUtility.Encrypt("123456")),
                    PIN = _cryptographyDataUtility.Encrypt("1234"),
                    Email = "",
                    NumberPhone = "",
                    DateC = DateTime.Now,
                    DateR = DateTime.Now,
                    DateV = DateTime.Now,
                    UserType = 0
                }
            );

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
