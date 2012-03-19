using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using System.Xml;
using DanbooruDownloader3.Entity;

namespace DanbooruDownloader3.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestShimmie2Parser()
        {
            DanbooruProviderDao pd = new DanbooruProviderDao();
            pd.xmlProviderUrl = @"../../../DanbooruDownloader3.test/DanbooruProviderList.xml";
            var shimmie2Provider = pd.GetAllProvider().Where<DanbooruProvider>(x => x.BoardType == BoardType.Shimmie2).First<DanbooruProvider>();
            XmlReader reader = new XmlTextReader(@"../../../DanbooruDownloader3.test/TestXml/shimmie2.xml");

            var list = ShimmieEngine.ParseRSS(reader, shimmie2Provider, "", "");

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 9);
        }
    }
}
