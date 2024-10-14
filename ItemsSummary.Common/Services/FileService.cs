using ItemsSummary.Core;

namespace ItemsSummary.Common.Services
{
    public class FileService : IFileService
    {
        private readonly IEnvironmentService _environmentService;
        public FileService(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public async Task<IEnumerable<string>> GetFilesByDaysAsync(string path, IEnumerable<string> days)
        {
            var files = new List<string>();
            await Task.Run(() =>
            {
                foreach (var day in days)
                {
                    files.AddRange(Directory.EnumerateFiles(path, $"{day}*.txt").OrderByDescending(f => f));
                }
            });
            return files;
        }

        public virtual string? GetOneDrivePath()
        {
            return _environmentService.GetEnvironmentVariable("OneDriveConsumer");
        }

        public string GetPoDirFullPath()
        {
            string? oneDrivePath = GetOneDrivePath();
            if (string.IsNullOrEmpty(oneDrivePath))
            {
                throw new KeyNotFoundException("Could not find OneDrive variable!");
            }
            string fullPath = Path.Combine(oneDrivePath, Constants.PO_RELATIVE_PATH);
            if (Directory.Exists(fullPath) == false)
            {
                throw new DirectoryNotFoundException();
            }
            return fullPath;
        }

        public virtual string? GetImageFilePath(string pid)
        {
            string? onedrivePath = GetOneDrivePath();
            if (onedrivePath == null || Directory.Exists(onedrivePath) == false) return null;

            string imagePath = $"{onedrivePath}\\{Constants.IMAGE_RELATIVE_PATH}\\{pid}.jpg";
            if (File.Exists(imagePath) == false) return null;

            return imagePath;
        }

        public async Task<IEnumerable<SummarizedItemInfo>> GetAllItemInfoByFilePathsAsync(IEnumerable<string> paths)
        {
            var allItemInfoCollection = Enumerable.Empty<SummarizedItemInfo>();
            foreach (var path in paths)
            {
                allItemInfoCollection = allItemInfoCollection.Concat(await GetItemsInfoFromFileAsync(path));
            }
            allItemInfoCollection = allItemInfoCollection
                                    .GroupBy(row => row.Pid)
                                    .Select(g => new SummarizedItemInfo()
                                    {
                                        Pid = g.Key,
                                        Quantity = g.Sum(i => i.Quantity),
                                        Set = g.Sum(i => i.Set),
                                        Pos = g.SelectMany(i => i.Pos)
                                    });
            return allItemInfoCollection;
        }
        public async Task<IEnumerable<PoInfo>> GetAllPoInfoByFilePathsAsync(IEnumerable<string> paths)
        {
            List<PoInfo> allPos = new List<PoInfo>();
            foreach (var path in paths)
            {
                var itemsInfo = await GetItemsInfoFromFileAsync(path);
                var singleItemsInfo = itemsInfo.Select(i => new SingleItemInfo()
                {
                    Pid = i.Pid,
                    Quantity = i.Quantity,
                    Set = i.Set,
                });
                PoInfo poInfo = new PoInfo()
                {
                    PoName = Path.GetFileNameWithoutExtension(path),
                    Items = singleItemsInfo,
                };
                allPos.Add(poInfo);
            }
            return allPos;
        }
        private async Task<IEnumerable<SummarizedItemInfo>> GetItemsInfoFromFileAsync(string path)
        {
            string txt = await _environmentService.ReadAllTextAsync(path);
            var lines = txt.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                throw new Exception($"注文から商品内容を取得できませんでした。{Constants.NL} {path}");
            }
            var itemInfoCollection = new List<SummarizedItemInfo>();
            foreach (var line in lines)
            {
                var blocks = line.Split("\t");
                if (blocks.Length != 4)
                {
                    throw new Exception($"注文の形式が正しくありません。{Constants.NL}{line}");
                }
                SummarizedItemInfo itemInfo = new SummarizedItemInfo()
                {
                    Pid = blocks[0],
                    Quantity = CorrectSpQuantityIfShopXorShopY(path, blocks[0], int.Parse(blocks[1])),
                    Set = int.Parse(blocks[1]),
                    Pos = [blocks[3]]
                };
                itemInfoCollection.Add(itemInfo);
            }
            return itemInfoCollection;
        }

        /// <summary>
        /// ショップX或いはショップYなら、Aシリーズの商品数量を2倍とする（入り数仕様のため）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pid"></param>
        /// <param name="originalQty"></param>
        /// <returns></returns>
        private int CorrectSpQuantityIfShopXorShopY(string path, string pid, int originalQty)
        {
            if ((path.Contains("_X_") || path.Contains("_Y_")) && pid.StartsWith("A"))
            {
                originalQty *= 2;
            }
            return originalQty;
        }


    }
}
