using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ItemsSummary.Core;

namespace ItemsSummary.ViewModel
{
    /// <summary>
    /// POファイルの情報VM
    /// </summary>
    public partial class PoFileInfoViewModel : ObservableObject
    {
        private readonly PoFileInfo _fileInfo;

        [ObservableProperty]
        private string poName;
        [ObservableProperty]
        private string fullPath;
        [ObservableProperty]
        private bool isChecked;
        [ObservableProperty]
        private bool isRecent;

        public PoFileInfoViewModel(PoFileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            PoName = _fileInfo.PoName;
            FullPath = _fileInfo.FullPath;
            IsRecent = _fileInfo.IsRecent;
        }
        /// <summary>
        /// 親VMにCheck状態が改変された旨を送信
        /// </summary>
        /// <param name="value"></param>
        partial void OnIsCheckedChanged(bool value)
        {
            WeakReferenceMessenger.Default.Send(new Messages.PoFileItemIsCheckedChangedMessage(value));
        }
        public override string ToString()
        {
            return $"IsChecked:{IsChecked}, {PoName}, IsRecent:{IsRecent}, FullPath:{FullPath}";
        }
    }
}
