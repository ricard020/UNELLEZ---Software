using ENTITYS;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.DatabaseRepository;
using REPOSITORY.DBContext;
using UTILITIES.CryptographyDataUtility;

namespace REPOSITORY.UserRepository
{
    public class UserRepository: IUserRepository
    {
        private static UserEntity CurrentUser { get; set; }

        private readonly IDatabaseRepositoy _databaseRepositoy;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICryptographyDataUtility _cryptographyDataUtility;

        public UserRepository(IServiceProvider serviceProvider, ICryptographyDataUtility cryptographyDataUtility, IDatabaseRepositoy databaseRepository)
        {
            _serviceProvider = serviceProvider;
            _cryptographyDataUtility = cryptographyDataUtility;
            _databaseRepositoy = databaseRepository;
        }

        public async Task<(bool, string, int)> AuthenticateUser(string coduser, string password)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    UserEntity userFound;
                    
                    if (_databaseRepositoy.GetIsOnlineValue())
                    {
                        var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                        userFound = await dbContext.Users.FirstOrDefaultAsync(e => e.Username == coduser);
                    }
                    else
                    {
                        var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>();
                        userFound = await dbContext.Users.FirstOrDefaultAsync(e => e.Username == coduser);
                    }

                    if (userFound != null)
                    {
                        if (userFound.DateV > DateTime.Now || userFound.UserType == 0 || userFound.UserType == 1)
                        {
                            var desencriptedPassword = _cryptographyDataUtility.Decrypt(userFound.Password);

                            if (password == desencriptedPassword) { return (true, "Inicio Exitoso", userFound.UserType); }
                            else { return (false, "Contraseña incorrecta.", userFound.UserType); }
                        }
                        else { return (false, "Membresía vencida.", userFound.UserType); }
                    }
                    else
                    {
                        return (false, "Código de usuario no encontrado", 0);
                    }
                }
                catch (Exception ex)
                {
                    return (false, ex.Message, 0);
                }
            }
        }

        public async Task<(bool, string)> AddUserLoggedToLocalBD(UserEntity newUser)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>();
                var transaction = dbContext.Database.BeginTransaction();

                try
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(e => e.Id == newUser.Id);
                    
                    if(user != null)
                        dbContext.Remove(user);

                    await dbContext.Users.AddAsync(newUser);

                    await dbContext.SaveChangesAsync();
                    
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


        public async Task<(string, bool, string)> ValidateUserEmailforUsernameRecovery(string email)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

                try
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(e => e.Email == email);

                    if (user != null)
                    {
                        return (user.Username, true, "");
                    }
                    else
                    {
                        return ("", false, "Usuario no encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    return ("", false, ex.Message);
                }
            }
        }

        public async Task<(bool, string)> UpdatePassword(string coduser, string newPassword)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                var transaction = dbContext.Database.BeginTransaction();

                try
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(e => e.Username == coduser);

                    if (user != null)
                    {
                        var newPasswordEncrypted = _cryptographyDataUtility.Encrypt(newPassword);

                        user.Password = newPasswordEncrypted;

                        dbContext.Update(user);
                    }
                    else
                    {
                        return (false, "Usuario no encontrado");
                    }

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
        
        public async Task<(bool, string)> Update(UserEntity newUserData)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                var transaction = dbContext.Database.BeginTransaction();

                try
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(e => e.Id == newUserData.Id);

                    if (user != null)
                    {
                        user.Username = newUserData.Username;
                        user.Descrip = newUserData.Descrip;
                        user.Password = newUserData.Password;
                        user.PIN = newUserData.PIN;
                        user.Email = newUserData.Email;
                        user.NumberPhone = newUserData.NumberPhone;
                        user.UserType = newUserData.UserType;

                        dbContext.Update(user);
                    }
                    else
                    {
                        return (false, "Usuario no encontrado");
                    }

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

        public async Task<(bool, string)> Add(UserEntity newUser)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                var transaction = dbContext.Database.BeginTransaction();

                try
                {
                    await dbContext.Users.AddAsync(newUser);

                    await dbContext.SaveChangesAsync();
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

        public async Task<(bool, string)> Delete(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                var transaction = dbContext.Database.BeginTransaction();

                try
                {
                    var user = await dbContext.Users.FirstAsync(e => e.Id == id);

                    dbContext.Remove(user);

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

        public async Task<(UserEntity, bool, string)> GetById(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                try
                {
                    var user = await dbContext.Users.FirstAsync(e => e.Id == id);

                    return (user, true, "");
                }
                catch (Exception ex)
                {
                    return (null, false, "");
                }
            }
        }
        
        public async Task<(UserEntity, bool, string)> GetByUserName(string username)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    UserEntity user;

                    if (_databaseRepositoy.GetIsOnlineValue())
                    {
                        var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

                        user = await dbContext.Users.FirstAsync(e => e.Username == username);
                    }
                    else
                    {
                        var dbContext = scope.ServiceProvider.GetService<SQLiteDBContext>();

                        user = await dbContext.Users.FirstAsync(e => e.Username == username);
                    }

                    if (user != null)
                        return (user, true, "");
                    else
                        return (null, false, "Usuario no encontrado");
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(ICollection<UserEntity>, bool, string)> GetAll()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                try
                {
                    var users = await dbContext.Users.ToListAsync();

                    return (users, true, "");
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(bool, bool, string)> VerifyMembershipValidity(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                try
                {
                    var user = await dbContext.Users.FirstAsync(e => e.Id == id);

                    if (user.DateV > DateTime.Now)
                        return (true, true, "");
                    else
                        return (false, true, "");
                }
                catch (Exception ex)
                {
                    return (false, false, ex.Message);
                }
            }
        }

        public async Task<(bool, string)> IncrementMembership(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                var transaction = dbContext.Database.BeginTransaction();

                try
                {
                    var user = await dbContext.Users.FirstAsync(e => e.Id == id);

                    if (user != null)
                    {
                        user.DateR = DateTime.Now;
                        user.DateV = user.DateV.AddMonths(1);

                        dbContext.Users.Update(user);

                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return (true, "");
                    }
                    else
                    {
                        return (false, "Usuario no encontrado");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, ex.Message);
                }
            }
        }

        public void SetCurrentUser(UserEntity currentUser)
        {
            CurrentUser = currentUser;
        }

        public UserEntity GetCurrentUser()
        {
            return CurrentUser;
        }

        public async Task<(ICollection<UserEntity>, bool, string)> GetAllTrainers()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
                try
                {
                    var users = await dbContext.Users.Where(e => e.UserType == 2 && e.DateV > DateTime.Now).ToListAsync();

                    return (users, true, "");
                }
                catch (Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(bool, int)> AuthenticateUserPIN(int userID, string PIN)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

                try
                {
                    var userFound = await dbContext.Users.FirstOrDefaultAsync(e => e.Id == userID);

                    if (userFound != null)
                    {
                        if (userFound.DateV > DateTime.Now || userFound.UserType == 0 || userFound.UserType == 1)
                        {
                            var desencriptedPin = _cryptographyDataUtility.Decrypt(userFound.PIN);

                            if (PIN == desencriptedPin)
                                return (true, 0);
                            else 
                                return (false, 1);
                        }
                        else 
                        {
                            return (false, 2);
                        }
                    }
                    else
                    {
                        return (false, 3);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return (false, 3);
                }
            }
        }
    }
}
