namespace T802.Services.Installation
{
    public interface IInstallationService
    {
        void InstallData(string username, string password, bool installSampleData = true);
        bool IsDatabaseInstalled();
    }
}
