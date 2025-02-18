using ENTITYS;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.DatabaseRepository;
using REPOSITORY.DBContext;

namespace REPOSITORY.ExerciseTemplateRepository
{
    public class ExerciseTemplateRepository : IExerciseTemplateRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseRepositoy _databaseRepositoy;

        public ExerciseTemplateRepository(IServiceProvider serviceProvider, IDatabaseRepositoy databaseRepositoy)
        {
            _serviceProvider = serviceProvider;
            _databaseRepositoy = databaseRepositoy;
        }

        public async Task<(ExerciseTemplateEntity, bool, string)> GetRestingExercise()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (_databaseRepositoy.GetIsOnlineValue())
                {
                    using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                    {
                        try
                        {
                            var exercise = await dbContext.ExerciseTemplate.FirstOrDefaultAsync(e => e.IsRestingExercise == (short)1);

                            return (exercise, true, "");
                        }
                        catch (Exception ex)
                        {
                            return (null, false, ex.InnerException.Message ?? ex.Message);
                        }
                    }
                }
                else
                {
                    using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                    {
                        try
                        {
                            var exercise = await dbContext.ExerciseTemplate.FirstOrDefaultAsync(e => e.IsRestingExercise == (short)1);

                            return (exercise, true, "");
                        }
                        catch (Exception ex)
                        {
                            return (null, false, ex.InnerException.Message ?? ex.Message);
                        }
                    }
                }
            }
        }

        public async Task<(bool, string)> SaveRestingExercise(ExerciseTemplateEntity newExercise)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                {
                    using (var transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var lastExercise = await dbContext.ExerciseTemplate.FirstOrDefaultAsync(e => e.IsRestingExercise == (short)1);

                            if(lastExercise != null)
                                dbContext.ExerciseTemplate.Remove(lastExercise);

                            dbContext.ExerciseTemplate.Add(newExercise);

                            await dbContext.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return (true, "");
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return (false, ex.InnerException.Message ?? ex.Message);
                        }
                    }
                }
            }
        }
    }
}
