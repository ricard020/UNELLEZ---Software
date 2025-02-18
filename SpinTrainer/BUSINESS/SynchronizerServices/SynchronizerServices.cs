using REPOSITORY.SessionRepository;

namespace SERVICES.SynchronizerServices
{
    public class SynchronizerServices : ISynchronizerServices
    {
        private readonly ISessionRepository _sessionRepository;

        public SynchronizerServices(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<(bool, string)> SynchronizeAsync()
        {
            try
            {
                var (sessionsCreateInLocalDb, operationComplete, errorMessage) = await _sessionRepository.GetAllSesionsInLocalDB();

                if (operationComplete)
                {
                    foreach (var session in sessionsCreateInLocalDb)
                    {
                        var sessionID = session.ID;
                        session.ID = 0;

                        var (insertComplete, insertErrorMessage) = await _sessionRepository.Add(session);

                        if (insertComplete)
                        {
                            var (deleteComplete, deleteErrorMessage) = await _sessionRepository.DeleteSessionInLocalDb(sessionID);

                            if (!deleteComplete)
                                return (false, "Error eliminando: " + deleteErrorMessage);
                        }
                        else
                        {
                            return (false, "Error insertando: " + insertErrorMessage);
                        }
                    }

                    return (true, "");
                }
                else
                {
                    return(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
