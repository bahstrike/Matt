using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Matt());

#if DEBUG
            // if we're done, then kill temp files
            foreach (string tmpfile in new string[] { Log.Filename })
            {
                try
                {
                    System.IO.File.Delete(tmpfile);
                }
                catch
                {

                }
            }
#endif

            Properties.Settings.Default.Save();
        }
    }
}
