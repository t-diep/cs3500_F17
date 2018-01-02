///
/// @authors Tony Diep and Sona Torosyan
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Contains the auto-generated executable to load up SpaceWarsGUI form
/// </summary>
namespace View
{
    static class View
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpaceWarsGUI());
        }
    }
}
