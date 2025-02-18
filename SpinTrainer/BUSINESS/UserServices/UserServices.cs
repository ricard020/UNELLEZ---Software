
using ENTITYS;
using REPOSITORY.UserRepository;

namespace SERVICES.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool, string)> Add(UserEntity newUser)
        {
            return await _userRepository.Add(newUser);
        }

        public async Task<(bool, string)> AddUserLoggedToLocalBD(UserEntity user)
        {
            return await _userRepository.AddUserLoggedToLocalBD(user);
        }

        public async Task<(bool, string, int)> AuthenticateUser(string username, string password)
        {
            return await _userRepository.AuthenticateUser(username, password);
        }

        public async Task<(bool, int)> AuthenticateUserPIN(int userID, string PIN)
        {
            return await _userRepository.AuthenticateUserPIN(userID, PIN);
        }

        public async Task<(bool, string)> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<(ICollection<UserEntity>, bool, string)> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<(ICollection<UserEntity>, bool, string)> GetAllTrainers()
        {
            return await _userRepository.GetAllTrainers();
        }

        public async Task<(UserEntity, bool, string)> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<(UserEntity, bool, string)> GetByUserName(string username)
        {
            return await _userRepository.GetByUserName(username);
        }

        public UserEntity GetCurrentUser()
        {
            return _userRepository.GetCurrentUser();
        }

        public async Task<(bool, string)> IncrementMembership(int id)
        {
            return await _userRepository.IncrementMembership(id);
        }

        public void SetCurrentUser(UserEntity currentUser)
        {
            _userRepository.SetCurrentUser(currentUser);
        }

        public async Task<(bool, string)> Update(UserEntity newUserData)
        {
            return await _userRepository.Update(newUserData);
        }

        public async Task<(bool, string)> UpdatePassword(string username, string password)
        {
            return await _userRepository.UpdatePassword(username, password);
        }

        public async Task<(string, bool, string)> ValidateUserEmailforUsernameRecovery(string email)
        {
            return await _userRepository.ValidateUserEmailforUsernameRecovery(email);
        }

        public async Task<(bool, bool, string)> VerifyMembershipValidity(int id)
        {
            return await _userRepository.VerifyMembershipValidity(id);
        }
    }
}
