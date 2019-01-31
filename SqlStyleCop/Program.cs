using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TST.SqlStyleCop;

namespace SqlStyleCop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            if (args == null || args.Length == 0)
                Application.Run(new MainWindow());
            else
            {
                if (args.Length < 2) throw new ArgumentException("Incorrect arguments.");
                var copNames = args.Skip(2).Select(a => a.ToString()).ToArray();
                var runManager = new StyleCopRunManager();
                runManager.Run(args[0].ToString(), args[1].ToString(), copNames);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogManager.GetLogger(typeof(Program)).Fatal(e.Exception);
            MessageBox.Show("An unrecoverable error occurs, the applicatoin will terminate. \r\n Error is logged under the executable folder.");
            Application.Exit();
        }
    }
}
