using CommunityToolkit.Mvvm.ComponentModel;
using ItemsSummary.Common;
using ItemsSummary.Core;

namespace ItemsSummary.ViewModel
{
    /// <summary>
    /// 数量が合算された後の商品のVM
    /// </summary>
    public partial class SummarizedItemInfoViewModel : ObservableObject
    {
        private readonly SummarizedItemInfo _itemInfo;

        [ObservableProperty]
        private bool isChecked;
        [ObservableProperty]
        private int index;
        [ObservableProperty]
        private string? imagePath;
        [ObservableProperty]
        private string pid;
        [ObservableProperty]
        private int quantity;
        [ObservableProperty]
        private int set;
        [ObservableProperty]
        private string pos;

        public SummarizedItemInfoViewModel(SummarizedItemInfo itemInfo)
        {
            _itemInfo = itemInfo;
            Pid = _itemInfo.Pid;
            Quantity = _itemInfo.Quantity;
            Set = _itemInfo.Set;
            pos = string.Join($"{Constants.NL}", _itemInfo.Pos);
        }
    }
}
