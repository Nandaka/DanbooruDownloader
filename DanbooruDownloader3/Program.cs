using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;

namespace DanbooruDownloader3
{
    static class Program
    {
        public static ILog Logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.GlobalContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd");
            log4net.Config.XmlConfigurator.Configure();
            if (Logger == null)
            {
                Logger = LogManager.GetLogger(typeof(FormMain));
            }
            //try
            //{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error("Unhandled Exception", ex);
            //    throw;
            //}
            Logger.Info("Closing down Danbooru Downloader.");
            Logger.Info("############################################################################");
        }
    }
}
