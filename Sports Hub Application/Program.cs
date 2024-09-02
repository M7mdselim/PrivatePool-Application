using System;
using System.Windows.Forms;

namespace Private_Pool_Application
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Login()); // Or your main form
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while starting the application: " + ex.Message);
            }
        }
    }
}
