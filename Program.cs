using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string path = null;
#if DEBUG
                path = "../TheTestEver/TheTestEver.ced";
#endif
                if (args.Length >= 1 && args[0] != null) path = args[0];
                for (int i = 1; i < args.Length; i++)
                {

                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new RedactorForm(path));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Can not execute resource redactor.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
