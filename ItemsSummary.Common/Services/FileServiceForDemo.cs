namespace ItemsSummary.Common.Services
{
    public class FileServiceForDemo : FileService
    {
        public FileServiceForDemo(IEnvironmentService environmentService) : base(environmentService)
        {
        }
        public override string? GetOneDrivePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
