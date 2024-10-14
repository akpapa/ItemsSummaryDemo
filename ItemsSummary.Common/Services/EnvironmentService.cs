
namespace ItemsSummary.Common.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public string? GetEnvironmentVariable(string varName)
        {
            return Environment.GetEnvironmentVariable(varName);
        }

        public Task<string> ReadAllTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }
    }
}
