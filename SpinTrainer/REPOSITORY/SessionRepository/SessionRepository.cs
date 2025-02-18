using ENTITYS;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.DatabaseRepository;
using REPOSITORY.DBContext;

namespace REPOSITORY.SessionRepository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseRepositoy _databaseRepositoy;

        public SessionRepository(IServiceProvider serviceProvider, IDatabaseRepositoy databaseRepositoy)
        {
            _serviceProvider = serviceProvider;
            _databaseRepositoy = databaseRepositoy;
        }

        public async Task<(bool, string)> Add(SessionEntity session)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (_databaseRepositoy.GetIsOnlineValue())
                {
                    using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                    {
                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                var sessionExercises = session.SessionExercises;
                                session.SessionExercises = null;

                                dbContext.Sessions.Add(session);

                                await dbContext.SaveChangesAsync();

                                foreach (var exercise in sessionExercises)
                                {
                                    exercise.ID = 0;
                                    exercise.Session = null;
                                    exercise.SessionID = session.ID;

                                    dbContext.SessionExercises.Add(exercise);

                                    await dbContext.SaveChangesAsync();
                                }

                                await transaction.CommitAsync();

                                return (true, "");
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                return (false, ex.InnerException.Message);
                            }
                        }
                    }
                }
                else
                {
                    using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                    {
                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                var sessionExercises = session.SessionExercises;
                                session.SessionExercises = null;

                                dbContext.Sessions.Add(session);

                                await dbContext.SaveChangesAsync();

                                foreach (var exercise in sessionExercises)
                                {
                                    exercise.Session = null;
                                    exercise.SessionID = session.ID;

                                    dbContext.SessionExercises.Add(exercise);

                                    await dbContext.SaveChangesAsync();
                                }

                                await transaction.CommitAsync();

                                return (true, "");
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                return (false, ex.InnerException.Message);
                            }
                        }
                    }
                }
            }
        }

        public async Task<(bool, string)> Delete(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (_databaseRepositoy.GetIsOnlineValue())
                {
                    using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                    {
                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                var session = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == id);

                                dbContext.Sessions.Remove(session);

                                await dbContext.SaveChangesAsync();

                                await transaction.CommitAsync();

                                return (true, "");
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                return (false, ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                    {
                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                var session = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == id);

                                dbContext.Sessions.Remove(session);

                                await dbContext.SaveChangesAsync();

                                await transaction.CommitAsync();

                                return (true, "");
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                return (false, ex.Message);
                            }
                        }
                    }
                }
            }
        }

        public async Task<(bool, string)> DeleteSessionInLocalDb(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                {
                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var session = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == id);

                            dbContext.Sessions.Remove(session);

                            await dbContext.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return (true, "");
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return (false, ex.Message);
                        }
                    }
                }
            }
        }

        public async Task<(ICollection<SessionEntity>, int, bool, string)> GetAllByFilters(DateTime? dateI, int trainerID, string? descrip, int skip)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    ICollection<SessionEntity> sessionsFound;

                    if (_databaseRepositoy.GetIsOnlineValue())
                    {
                        using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                        {
                            sessionsFound = await dbContext.Sessions.Include(e => e.SessionExercises).Where(e => e.TrainerID == trainerID &&
                                                                                                        (dateI == null ||
                                                                                                        DateOnly.FromDateTime(e.DateI) == DateOnly.FromDateTime(dateI == null ? DateTime.Now : new DateTime(dateI.Value.Year, dateI.Value.Month, dateI.Value.Day))) &&
                                                                                                        (string.IsNullOrEmpty(descrip) ||
                                                                                                        (e.Descrip ?? string.Empty).ToLower().Contains(descrip.ToLower())))
                                                                                                 .Skip(skip)
                                                                                                 .Take(5)
                                                                                                 .ToListAsync();
                        }
                    }
                    else
                    {
                        using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                        {
                            sessionsFound = await dbContext.Sessions.Include(e => e.SessionExercises).Where(e => e.TrainerID == trainerID &&
                                                                                                        (dateI == null ||
                                                                                                        DateOnly.FromDateTime(e.DateI) == DateOnly.FromDateTime(dateI == null ? DateTime.Now : new DateTime(dateI.Value.Year, dateI.Value.Month, dateI.Value.Day))) &&
                                                                                                        (string.IsNullOrEmpty(descrip) ||
                                                                                                        (e.Descrip ?? string.Empty).ToLower().Contains(descrip.ToLower())))
                                                                                                 .Skip(skip)
                                                                                                 .Take(5)
                                                                                                 .ToListAsync();
                        }
                    }

                    return (sessionsFound, sessionsFound.Count(), true, "");
                }
                catch (Exception ex)
                {
                    return (null, 0, false, ex.Message);
                }
            }
        }

        public async Task<(ICollection<SessionEntity>, bool, string)> GetAllSesionsInLocalDB()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                    {
                        var sessionsFound = await dbContext.Sessions.Include(e => e.SessionExercises).ToListAsync();

                        return (sessionsFound, true, "");
                    }
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(SessionEntity, bool, string)> GetByID(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    SessionEntity sessionFound;
                    if (_databaseRepositoy.GetIsOnlineValue())
                    {
                        using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                        {
                            sessionFound = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == id);
                        }
                    }
                    else
                    {
                        using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                        {
                            sessionFound = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == id);
                        }
                    }

                    return (sessionFound, true, "");
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(bool, string)> Update(SessionEntity session)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (_databaseRepositoy.GetIsOnlineValue())
                {
                    using (var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>())
                    {
                        using (var transaction = dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                /*Eliminar la sesion vieja*/
                                var sessionToDeleted = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == session.ID);

                                dbContext.Sessions.Remove(sessionToDeleted);

                                session.ID = 0;

                                var sessionExercises = session.SessionExercises;
                                session.SessionExercises = null;

                                dbContext.Sessions.Add(session);

                                await dbContext.SaveChangesAsync();

                                foreach (var exercise in sessionExercises)
                                {
                                    exercise.Session = null;
                                    exercise.ID = 0;
                                    exercise.SessionID = session.ID;

                                    dbContext.SessionExercises.Add(exercise);

                                    await dbContext.SaveChangesAsync();
                                }

                                await dbContext.SaveChangesAsync();
                                await transaction.CommitAsync();

                                return (true, "");
                            }
                            catch (Exception ex)
                            {
                                transaction.RollbackAsync();
                                return (false, ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    using (var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>())
                    {
                        using (var transaction = dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                /*Eliminar la sesion vieja*/
                                var sessionToDeleted = await dbContext.Sessions.Include(e => e.SessionExercises).FirstOrDefaultAsync(e => e.ID == session.ID);

                                dbContext.Sessions.Remove(sessionToDeleted);

                                session.ID = 0;

                                var sessionExercises = session.SessionExercises;
                                session.SessionExercises = null;

                                dbContext.Sessions.Add(session);

                                await dbContext.SaveChangesAsync();

                                foreach (var exercise in sessionExercises)
                                {
                                    exercise.Session = null;
                                    exercise.SessionID = session.ID;

                                    dbContext.SessionExercises.Add(exercise);

                                    await dbContext.SaveChangesAsync();
                                }

                                await dbContext.SaveChangesAsync();
                                await transaction.CommitAsync();

                                return (true, "");
                            }
                            catch (Exception ex)
                            {
                                transaction.RollbackAsync();
                                return (false, ex.Message);
                            }
                        }
                    }
                }
            }
        }
    }
}
