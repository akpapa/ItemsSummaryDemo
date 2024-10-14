using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ItemsSummary.ViewModel.Messages
{
    /// <summary>
    /// POファイルVMのCheck状態が変更されたときに送られたMessageを定義する
    /// </summary>
    internal class PoFileItemIsCheckedChangedMessage : ValueChangedMessage<bool>
    {
        public PoFileItemIsCheckedChangedMessage(bool value) : base(value)
        {
        }
    }
}
