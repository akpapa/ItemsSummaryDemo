namespace ItemsSummary.Core.Test
{
    [TestClass]
    public class PoFileInfoTests
    {
        [TestMethod]
        public void PoFileInfo_Should_Return_Correct_FileName()
        {
            //arrange
            PoFileInfo info = new PoFileInfo() { FullPath = "D:\\Test\\20240614_X_Name_5000済.txt" };
            string expect = "20240614_X_Name_5000済";
            //action
            string actual = info.PoName;
            //assert
            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void PoFileInfo_IsRecent_Should_Return_Correctly()
        {
            //arrange
            PoFileInfo infoRecent = new PoFileInfo() { FullPath = $"D:\\Test\\{DateTime.Today:yyyyMMdd}_X_Name_5000済.txt" };
            PoFileInfo infoNotRecent = new PoFileInfo() { FullPath = "D:\\Test\\20240614_X_Name_5000済.txt" };

            //action
            bool isRecent = infoRecent.IsRecent;
            bool isNotRecent = infoNotRecent.IsRecent;
            //assert
            Assert.IsTrue(isRecent && !isNotRecent);
        }
    }
}
