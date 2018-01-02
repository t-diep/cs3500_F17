///
/// @author Tony Diep and Sona Torosyan 
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Contains the models that make up the Spreadsheet GUI
/// </summary>
namespace SpreadsheetGUI
{
    public class SpreadsheetApplication : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplication appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplication()
        {
            
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadsheetApplication GetAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetApplication();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }

    /// <summary>
    /// Blueprint for Spreadsheet GUI
    /// </summary>
    static class SpreadsheetGUI
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpreadsheetForm());
        }
    }
}
