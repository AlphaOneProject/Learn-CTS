using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    static class Program
    {
        /// <summary>
        /// Entry point of the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Menu());
            Application.Run(new PNCWindow("Learn CTS", "1.Niveau de découverte", "1.Enlèvement"));
        }
    }
}
