namespace ItemsSummary.Common.Services
{
    public interface IEnvironmentService
    {
        string? GetEnvironmentVariable(string varName);
        Task<string> ReadAllTextAsync(string path);
    }
}
