using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResrouceRedactor
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                string path = null;
                if (args.Length >= 1 && args[0] != null) path = args[0];

                var form = new RedactorForm(path);
                for (int i = 1; i < args.Length - 1; i++)
                {
                    switch (args[i++])
                    {
                        case "-maximized": if (args[i] == "true") form.WindowState = FormWindowState.Maximized; break;
                    }
                }
                Application.Run(form);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Can not execute resource redactor.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
