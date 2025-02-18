namespace SERVICES.SynchronizerServices
{
    public interface ISynchronizerServices
    {
        Task<(bool,string)> SynchronizeAsync();
    }
}
