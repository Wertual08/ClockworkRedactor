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
                for (int i = 0; i < args.Length; i++)
                {

                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new RedactorForm("../TheTestEver/TheTestEver.ced"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Can not execute resource redactor.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
