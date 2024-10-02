using System.Windows.Forms;
using System;

namespace Private_Pool_Application
{
    public static class Program  // Class should be public
    {
        [STAThread]
        public static void Main()  // Main method should also be public
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Login());  // Entry point for the application
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while starting the application: " + ex.Message);
            }
        }
    }
}

