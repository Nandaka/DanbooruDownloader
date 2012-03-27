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

namespace DanbooruDownloader3.test
{
    [TestClass]
    public class UnitTest1
    {
        string source = @"../../../DanbooruDownloader3.test/DanbooruProviderList.xml";

        [TestMethod]
        public void TestShimmie2Parser()
        {
            DanbooruProviderDao pd = new DanbooruProviderDao();
            var shimmie2Provider = pd.Read(source).Where<DanbooruProvider>(x => x.BoardType == BoardType.Shimmie2).First<DanbooruProvider>();
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
            var list = pd.Read(source);
            list[0].Name = "hahaha";
            pd.Save(list, target);
            Assert.IsTrue(System.IO.File.Exists(target));
            XDocument doc = XDocument.Load(target);
            list = pd.Read(target);
            Assert.IsTrue(list[0].Name == "hahaha");
        }
    }
}
