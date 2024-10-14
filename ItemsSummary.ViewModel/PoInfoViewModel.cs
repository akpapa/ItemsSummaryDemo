using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemsSummary.ViewModel
{
    /// <summary>
    /// 単一の注文内容VM
    /// </summary>
    public partial class PoInfoViewModel : ObservableObject
    {
        private readonly Core.PoInfo _poInfo;

        [ObservableProperty]
        private string poName;
        [ObservableProperty]
        private string summary;
        [ObservableProperty]
        IEnumerable<SingleItemInfoViewModel> items;

        public PoInfoViewModel(Core.PoInfo poInfo)
        {
            _poInfo = poInfo;
            PoName = _poInfo.PoName;
            Summary = _poInfo.Summary;
            Items = _poInfo.Items.Select(i => new SingleItemInfoViewModel(i)).ToList();//IEnumerableのままだとforeachで改変できない
        }
    }
}
