using DanbooruDBProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DanbooruDownloader3.test
{
    [TestClass]
    public class UnitTest2
    {
        private SQLiteProvider provider = null;

        [TestInitialize]
        public void Prepare()
        {
            var path = @"c:\temp\test.sqlite";
            if (File.Exists(path))
                File.Delete(path);
            provider = new SQLiteProvider(path);
        }

        [TestMethod]
        public void TestMethod1()
        {
            {
                var result = provider.Create();

                Assert.IsTrue(result, "Create DB");
            }
            {
                var result = provider.Insert("provider_name1", "post_id2", "filename3", "path4");
                Assert.IsTrue(result, "Insert DB");
                result = provider.Insert("provider_name12", "post_id22", "filename32", "path42");
                Assert.IsTrue(result, "Insert DB");

                var entries = provider.GetDownloadedFileByProviderAndPostId("provider_name1", "post_id2");

                Assert.IsNotNull(entries[0]);
                Assert.AreEqual(entries[0].Filename, "filename3");
                Assert.AreEqual(entries[0].Path, "path4");
            }
        }
    }
}