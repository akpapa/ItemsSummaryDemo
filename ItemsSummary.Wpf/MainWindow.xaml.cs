using CommunityToolkit.Mvvm.Messaging;
using ItemsSummary.ViewModel;
using System.Windows;

namespace ItemsSummary.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                if (DataContext == null) return;
                //MainWindowロード時にPO商品一覧を表示する
                await ((MainWindowViewModel)DataContext).ReloadPoFileToListViewCommand.ExecuteAsync(this);
            };
            ///MessageBoxを表示する
            ///MessageBoxの表示はViewの仕事と考え、VMからのMessageにて指示を受けてViewで表示する
            WeakReferenceMessenger.Default.Register<ViewModel.Messages.MessageBoxMessage>(this, (r, m) =>
            {
                var icon = MessageBoxImage.Information;
                if (m.Value.level == Common.Constants.MessageLevel.Warning) { icon = MessageBoxImage.Warning; }
                if (m.Value.level == Common.Constants.MessageLevel.Error) { icon = MessageBoxImage.Error; }
                MessageBox.Show(m.Value.msg, m.Value.caption, MessageBoxButton.OK, icon);
            });
        }
    }
}