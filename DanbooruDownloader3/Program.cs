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
            PrepareLogger();
            SetLogger(DanbooruDownloader3.Properties.Settings.Default.EnableLogging);            

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

        private static void PrepareLogger()
        {
            log4net.GlobalContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd");
            Logger = LogManager.GetLogger(typeof(FormMain));
        }

        public static void SetLogger(bool isEnabled)
        {
            if (isEnabled)
            {
                log4net.Config.XmlConfigurator.Configure();
                Program.Logger.Logger.Repository.Threshold = log4net.Core.Level.All;
                Logger.Info("Logging Enabled");
            }
            else
            {
                Logger.Info("Logging Disabled");
                Program.Logger.Logger.Repository.Threshold = log4net.Core.Level.Off;
            }
        }
    }
}
