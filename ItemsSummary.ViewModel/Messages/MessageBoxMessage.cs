using CommunityToolkit.Mvvm.Messaging.Messages;
using ItemsSummary.Common;

namespace ItemsSummary.ViewModel.Messages
{
    /// <summary>
    /// Viewにメッセージを表示させるためのMessageを定義する
    /// </summary>
    public class MessageBoxMessage : ValueChangedMessage<(Constants.MessageLevel level, string msg, string caption)>
    {
        public MessageBoxMessage((Constants.MessageLevel level, string msg, string caption) value) : base(value)
        {
        }
    }
}
