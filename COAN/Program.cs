using System;
using System.Windows.Forms;
using NLog;

namespace COAN
{
    public static class Program
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            logger.Log(LogLevel.Trace, "App open");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
