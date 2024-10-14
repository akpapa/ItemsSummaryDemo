using ItemsSummary.Common.Services;
using ItemsSummary.Core;
namespace ItemsSummary.ViewModel.Test
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public async Task MainWindowViewModel_PoFileList_Should_Be_Filled_After_Reload_Command()
        {
            MainWindowViewModel vm = new MainWindowViewModel(new FileServiceForDemo(new EnvironmentService()));
            await vm.ReloadPoFileToListViewCommand.ExecuteAsync(null);

            Assert.IsTrue(vm.PoFileList != null && vm.PoFileList.Any());
            foreach (PoFileInfoViewModel item in vm.PoFileList)
            {
                if (item.IsRecent && !item.IsChecked)
                {
                    Assert.Fail($"IsRecent and IsChecked do not match! {item}");
                }
            }
        }

        [TestMethod]
        public void PoFileInfoViewModel_Properties_Should_Match_PoFileInfo()
        {
            PoFileInfo poFileInfo = new PoFileInfo() { FullPath = "D:\\Test\\20240614_X_Name_5000済.txt" };
            MainWindowViewModel mainVm = new MainWindowViewModel(new FileServiceForDemo(new EnvironmentService()));
            PoFileInfoViewModel item = new PoFileInfoViewModel(poFileInfo);

            Assert.AreEqual(poFileInfo.PoName, item.PoName);
            Assert.AreEqual(poFileInfo.FullPath, item.FullPath);
            Assert.AreEqual(poFileInfo.IsRecent, item.IsRecent);
        }
    }
}
