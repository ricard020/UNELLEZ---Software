using ENTITYS;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.DatabaseRepository;
using REPOSITORY.DBContext;

namespace REPOSITORY.ExerciseRepository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly IDatabaseRepositoy _databaseRepositoy;
        private readonly IServiceProvider _serviceProvider;
        
        public ExerciseRepository(IServiceProvider serviceProvider, IDatabaseRepositoy databaseRepositoy)
        {
            _serviceProvider = serviceProvider;

            _databaseRepositoy = databaseRepositoy;
        }

        public async Task<(ExerciseEntity, bool, string)> GetById(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    ExerciseEntity exercise;

                    if (_databaseRepositoy.GetIsOnlineValue())
                    {
                        var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

                        exercise = await dbContext.Exercises.FirstOrDefaultAsync(e => e.ID == id);
                    }
                    else
                    {
                        var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>();

                        exercise = await dbContext.Exercises.FirstOrDefaultAsync(e => e.ID == id);
                    }
                    

                    return (exercise, true, "");
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(IEnumerable<ExerciseEntity>, bool, string)> GetAll()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    IEnumerable<ExerciseEntity> exercises;

                    if (_databaseRepositoy.GetIsOnlineValue())
                    {
                        var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

                        exercises = await dbContext.Exercises.ToListAsync();
                    }
                    else
                    {
                        var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>();

                        exercises = await dbContext.Exercises.ToListAsync();
                    }

                    return (exercises, true, "");
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }
    }
}