using ENTITYS;
using REPOSITORY.SessionRepository;
using System.Collections.ObjectModel;

namespace SERVICES.SessionServices
{
    public class SessionServices : ISessionServices
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionServices(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<(bool, string)> Add(SessionEntity session)
        {
            return await _sessionRepository.Add(session);
        }

        public async Task<(bool, string)> Delete(int id)
        {
            return await _sessionRepository.Delete(id);
        }

        public Task<(bool, string)> DeleteSessionInLocalDb(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<(ObservableCollection<SessionEntity>, int, bool, string)> GetAllByFilters(DateTime? dateI, int idEntrenador, string? descrip, int skip)
        {
            var (sessionsCollection, recordsFound, operationComplete, errorMessage) = await _sessionRepository.GetAllByFilters(dateI, idEntrenador, descrip, skip);

            var sessions = new ObservableCollection<SessionEntity>(sessionsCollection.Take(4));

            return (sessions, recordsFound, operationComplete, errorMessage);
        }

        public async Task<(SessionEntity, bool, string)> GetByID(int id)
        {
            return await _sessionRepository.GetByID(id);
        }

        public async Task<(bool, string)> Update(SessionEntity session)
        {
            return await _sessionRepository.Update(session);
        }
    }
}
