namespace ItemsSummary.Common.Services
{
    public interface IFileService
    {
        string? GetOneDrivePath();
        string GetPoDirFullPath();
        string? GetImageFilePath(string pid);
        Task<IEnumerable<string>> GetFilesByDaysAsync(string path, IEnumerable<string> days);
        Task<IEnumerable<Core.SummarizedItemInfo>> GetAllItemInfoByFilePathsAsync(IEnumerable<string> paths);
        Task<IEnumerable<Core.PoInfo>> GetAllPoInfoByFilePathsAsync(IEnumerable<string> paths);

    }
}
