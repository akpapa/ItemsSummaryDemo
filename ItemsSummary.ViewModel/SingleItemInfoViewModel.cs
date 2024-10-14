using CommunityToolkit.Mvvm.ComponentModel;
using ItemsSummary.Core;
namespace ItemsSummary.ViewModel
{
    /// <summary>
    /// 注文の中の単独商品(数量合算される前)の商品VM
    /// </summary>
    public partial class SingleItemInfoViewModel : ObservableObject
    {
        private readonly SingleItemInfo _singleItemInfo;
        [ObservableProperty]
        private bool isChecked;
        [ObservableProperty]
        private string? imagePath;
        [ObservableProperty]
        private string pid;
        [ObservableProperty]
        private int quantity;
        [ObservableProperty]
        private int set;
        [ObservableProperty]
        private string? po;
        public SingleItemInfoViewModel(SingleItemInfo singleItemInfo)
        {
            _singleItemInfo = singleItemInfo;
            Pid = singleItemInfo.Pid;
            Quantity = singleItemInfo.Quantity;
            Set = singleItemInfo.Set;
        }
    }
}
