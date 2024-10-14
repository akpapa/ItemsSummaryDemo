using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ItemsSummary.Common.Extensions;
using ItemsSummary.Common.Services;
using ItemsSummary.Core;

namespace ItemsSummary.ViewModel
{
    /// <summary>
    /// MainWindowのVM
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IFileService _fileService;

        //注文一覧
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoadSelectedPoToListViewCommand))]
        private IEnumerable<PoFileInfoViewModel>? poFileList;

        //数量が合算された商品リスト
        [ObservableProperty]
        private IEnumerable<SummarizedItemInfoViewModel>? itemInfoList;

        //注文別情報リスト
        [ObservableProperty]
        private IEnumerable<PoInfoViewModel>? poInfoList;

        //選択されあ注文の商品まとめ情報
        [ObservableProperty]
        private string? itemsSummary;

        public MainWindowViewModel(IFileService fileService)
        {
            _fileService = fileService;
            WeakReferenceMessenger.Default.Register<Messages.PoFileItemIsCheckedChangedMessage>(this, (r, m) =>
            {//PoListの子供ItemのIsCheckedが変わった時にMessageを受信して、ボタンのEnabledを変える
                LoadSelectedPoToListViewCommand.NotifyCanExecuteChanged();
            });
        }

        /// <summary>
        /// 直近7日間の注文一覧リストを取得し、今日と昨日の注文をChecked状態にする
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task ReloadPoFileToListView()
        {
            try
            {
                //Demoのために未来の日付まで対応
                var sevenDays = DateTime.Today.AddDays(2).GetLastDaysStrings(7);
                string dirPath = _fileService.GetPoDirFullPath();

                var pofiles = await _fileService.GetFilesByDaysAsync(dirPath, sevenDays);
                var poInfoListViewItems = pofiles.Select(p => new PoFileInfoViewModel(new PoFileInfo() { FullPath = p })).ToList();

                foreach (var item in poInfoListViewItems)
                {//最近の注文＝今日か昨日だけチェックする
                    item.IsChecked = item.IsRecent;
                }
                PoFileList = poInfoListViewItems;
                if (poInfoListViewItems.Count == 0)
                {//View側にエラーメッセージを表示させる
                    WeakReferenceMessenger.Default.Send(new Messages.MessageBoxMessage((Common.Constants.MessageLevel.Information, "注文ファイルが存在しません。", "情報")));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new Messages.MessageBoxMessage((Common.Constants.MessageLevel.Error, ex.Message, "エラー")));
            }
        }

        /// <summary>
        /// 選択された注文の合算された商品一覧を表示させる 
        /// </summary>
        /// <returns></returns>

        [RelayCommand(CanExecute = nameof(CanLoadSelectedPo))]
        private async Task LoadSelectedPoToListView()
        {
            try
            {
                if (PoFileList == null) { return; }
                var allPos = PoFileList.Where(f => f.IsChecked).Select(f => f.FullPath);
                //合算された商品VMリスト
                var allItemInfoVm =
                    (await _fileService.GetAllItemInfoByFilePathsAsync(allPos))
                    .Select(i => new SummarizedItemInfoViewModel(i)
                    { ImagePath = _fileService.GetImageFilePath(i.Pid) })
                    .ToList();

                for (int i = 0; i < allItemInfoVm.Count; i++)
                {//商品番号は１から数える
                    allItemInfoVm[i].Index = i + 1;
                }
                ItemInfoList = allItemInfoVm;

                //選択された注文のサマリ情報
                ItemsSummary = $"選択した注文数：{PoFileList.Where(p => p.IsChecked).Count()}, 商品種類数：{ItemInfoList.Count()}, 商品袋数：{ItemInfoList.Sum(i => i.Quantity)}, 商品セット数:{ItemInfoList.Sum(i => i.Set)}";


                //RAW注文VMリスト
                var allPoInfoVm = (await _fileService.GetAllPoInfoByFilePathsAsync(allPos))
                                  .Select(p => new PoInfoViewModel(p))
                                  .ToList();

                foreach (var po in allPoInfoVm)
                {
                    foreach (var item in po.Items)
                    {
                        item.ImagePath = _fileService.GetImageFilePath(item.Pid);
                        item.Po = po.PoName;
                    }
                }

                PoInfoList = allPoInfoVm;
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new Messages.MessageBoxMessage((Common.Constants.MessageLevel.Error, ex.Message, "エラー")));
            }
        }

        /// <summary>
        /// 実行ボタンのEnable状態を決める
        /// </summary>
        /// <returns></returns>
        private bool CanLoadSelectedPo()
        {
            if (PoFileList == null) { return false; }
            bool isNotEmpty = PoFileList.Any();
            bool hasSelected = false;
            foreach (var po in PoFileList)
            {
                if (po.IsChecked)
                {
                    hasSelected = true;
                    break;
                }
            }
            return isNotEmpty && hasSelected;
        }
    }
}
