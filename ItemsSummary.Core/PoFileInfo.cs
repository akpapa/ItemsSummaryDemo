namespace ItemsSummary.Core
{
    /// <summary>
    /// 注文ファイルのModel
    /// </summary>
    public record class PoFileInfo
    {
        public string PoName
        {
            get => Path.GetFileNameWithoutExtension(FullPath);
        }
        public required string FullPath { get; set; }
        /// <summary>
        /// 今日か昨日はRecentとする
        /// </summary>
        public bool IsRecent
        {
            get => FullPath.Contains(DateTime.Today.ToString("yyyyMMdd"), StringComparison.OrdinalIgnoreCase)
                  || FullPath.Contains(DateTime.Today.AddDays(-1).ToString("yyyyMMdd"), StringComparison.OrdinalIgnoreCase);
        }

    }
}
