namespace rapid_docs_services.Services.System
{
    public interface ISystemService
    {
        Task CleanSignings();
        Task CleanBlobStorage();
    }
}
