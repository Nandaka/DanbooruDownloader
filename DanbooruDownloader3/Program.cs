using DanbooruDBProvider;
using log4net;
using System;
using System.Windows.Forms;

namespace DanbooruDownloader3
{
    internal static class Program
    {
        public static ILog Logger;
        public static readonly SQLiteProvider DB = new SQLiteProvider("downloaded.sqlite");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            PrepareLogger();
            SetLogger(DanbooruDownloader3.Properties.Settings.Default.EnableLogging);

#if !DEBUG
            try
            {
#endif
                DB.Create();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
#if !DEBUG
            }
            catch (Exception ex)
            {
                Logger.Error("Unhandled Exception", ex);
                Logger.Error("Terminating Danbooru Downloader .");
                Logger.Error("############################################################################");
                throw;
            }
#endif
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