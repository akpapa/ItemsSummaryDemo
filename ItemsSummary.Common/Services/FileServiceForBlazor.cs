namespace ItemsSummary.Common.Services
{
    public class FileServiceForBlazor : FileService
    {
        public FileServiceForBlazor(IEnvironmentService environmentService) : base(environmentService)
        {
        }
        public override string? GetOneDrivePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }
        public override string? GetImageFilePath(string pid)
        {
            return $"/imgs/{pid}.jpg";
        }
    }
}
