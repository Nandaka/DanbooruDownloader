using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using System.Xml;
using DanbooruDownloader3.Entity;
using System.Xml.Linq;
using System.ComponentModel;

namespace DanbooruDownloader3.test
{
    [TestClass]
    public class UnitTest1
    {
        string sourceProvider = @"../../../DanbooruDownloader3.test/DanbooruProviderList.xml";
        string sourceDanbooruXml = @"../../../DanbooruDownloader3.test/TestXml/danbooru.xml";
        string sourceYandereXml = @"../../../DanbooruDownloader3.test/TestXml/yande.re.xml";

        [TestMethod]
        public void TestShimmie2Parser()
        {
            DanbooruProviderDao pd = new DanbooruProviderDao();
            var shimmie2Provider = pd.Read(sourceProvider).Where<DanbooruProvider>(x => x.BoardType == BoardType.Shimmie2).First<DanbooruProvider>();
            XmlReader reader = new XmlTextReader(@"../../../DanbooruDownloader3.test/TestXml/shimmie2.xml");

            var list = ShimmieEngine.ParseRSS(reader, shimmie2Provider, "", "");

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 9);
        }

        [TestMethod]
        public void TestProviderSave()
        {            
            string target = @"../../../DanbooruDownloader3.test/testSave.xml";
            DanbooruProviderDao pd = new DanbooruProviderDao();
            var list = pd.Read(sourceProvider);
            list[0].Name = "hahaha";
            pd.Save(list, target);
            Assert.IsTrue(System.IO.File.Exists(target));
            XDocument doc = XDocument.Load(target);
            list = pd.Read(target);
            Assert.IsTrue(list[0].Name == "hahaha");
        }

        [TestMethod]
        public void TestDanbooruEngineParser()
        {
            DanbooruProviderDao pd = new DanbooruProviderDao();
            DanbooruXmlEngine e = new DanbooruXmlEngine();

            {
                XDocument doc = XDocument.Load(sourceDanbooruXml);
                var searchQuery = new DanbooruSearchParam();
                searchQuery.Provider = pd.Read(sourceProvider).Where<DanbooruProvider>(x => x.BoardType == BoardType.Danbooru && x.Name.Contains("danbooru")).First<DanbooruProvider>();
                BindingList<DanbooruPost> result = e.Parse(doc.ToString(), searchQuery);
                Assert.IsNotNull(result);
                Assert.IsNotNull(e.RawData);
                Assert.IsTrue(e.TotalPost == 1021107);
                Assert.IsTrue(result.Count == 20);
                Assert.IsTrue(result[0].PreviewUrl == "http://danbooru.donmai.us/ssd/data/preview/73531fc4dda535ef87e57df633caf756.jpg");
            }

            {
                XDocument doc = XDocument.Load(sourceYandereXml);
                var searchQuery = new DanbooruSearchParam();
                searchQuery.Provider = pd.Read(sourceProvider).Where<DanbooruProvider>(x => x.BoardType == BoardType.Danbooru && x.Name.Contains("yande.re")).First<DanbooruProvider>();
                BindingList<DanbooruPost> result = e.Parse(doc.ToString(), searchQuery);
                Assert.IsNotNull(result);
                Assert.IsNotNull(e.RawData);
                Assert.IsTrue(e.TotalPost == 160753);
                Assert.IsTrue(result.Count == 16);
                Assert.IsTrue(result[0].PreviewUrl == "https://yande.re/data/preview/d3/41/d34184030ee19c6e63051967cf135f65.jpg");
            }
        }
    }
}
