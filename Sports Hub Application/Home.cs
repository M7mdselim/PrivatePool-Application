using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Private_Pool_Application
{
    public partial class Home : Form
    {
        private float _initialFormWidth;
        private float _initialFormHeight;
        private ControlInfo[] _controlsInfo;
        private string _username;
        

        public Home(string username)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during InitializeComponent: {ex.Message}");
                return;
            }

            // Check if username is valid, else exit
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Invalid login attempt. Username is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            _username = username;
            

            // Debug message for initialization
            Console.WriteLine($"Home initialized with username: {_username}");

            // Store initial form size for resizing controls
            _initialFormWidth = this.Width;
            _initialFormHeight = this.Height;
            this.Load += Home_Load; // Ensure the event handler is attached
            // Set event handler for form resize
            this.Resize += Home_Resize;
        }

        private void SetButtonVisibilityBasedOnRole()
        {
            Console.WriteLine($"_username value: {_username}");

            // Fetch role based on the logged-in user
            int roleID = GetRoleIdForCurrentUser();

            Console.WriteLine($"RoleID for user {_username}: {roleID}");

            // Control visibility of buttons based on role
            switch (roleID)
            {
                case 1: // Case for specific roles, e.g., admin or superuser
                case 3:
                    signupbtn.Visible = false;
                    changepassbtn.Visible = false;
                    break;
                case 4: // Example role where the user has access to both buttons
                    signupbtn.Visible = true;
                    changepassbtn.Visible = true;
                    break;
                default:
                    Console.WriteLine("Unknown RoleID. Button visibility not modified.");
                    break;
            }
        }

        private int GetRoleIdForCurrentUser()
        {
            Console.WriteLine("GetRoleIdForCurrentUser called."); // Debug message
            int roleID = 4;

            if (string.IsNullOrEmpty(_username))
            {
                Console.WriteLine("Logged-in username is not available.");
                return roleID;
            }

            if (string.IsNullOrEmpty(DatabaseConfig.connectionString))
            {
                Console.WriteLine("Database connection string is not configured properly.");
                return roleID;
            }

            Console.WriteLine($"Attempting to get role for username: {_username}");
            Console.WriteLine($"Connection String: {DatabaseConfig.connectionString}");

            string query = @"
    SELECT RoleID
    FROM Mixedgym.dbo.CashierDetails
    WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = _username;

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out roleID))
                        {
                            return roleID;
                        }
                        else
                        {
                            Console.WriteLine("No result returned from query.");
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            Console.WriteLine($"Error Number: {error.Number}, Message: {error.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"General Exception: {ex.Message}");
                    }
                }
            }

            return roleID; // Return default roleID if no result is found
        }


        private void Home_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Home_Load event triggered.");
            // Set button visibility based on the user's role
            SetButtonVisibilityBasedOnRole();
        }

        private void Home_Resize(object sender, EventArgs e)
        {
            float widthRatio = this.Width / _initialFormWidth;
            float heightRatio = this.Height / _initialFormHeight;
            ResizeControls(this.Controls, widthRatio, heightRatio);
        }

        private void ResizeControls(Control.ControlCollection controls, float widthRatio, float heightRatio)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control control = controls[i];
                ControlInfo controlInfo = _controlsInfo[i];

                control.Left = (int)(controlInfo.Left * widthRatio);
                control.Top = (int)(controlInfo.Top * heightRatio);
                control.Width = (int)(controlInfo.Width * widthRatio);
                control.Height = (int)(controlInfo.Height * heightRatio);

                // Adjust font size proportionally
                control.Font = new Font(control.Font.FontFamily, controlInfo.FontSize * Math.Min(widthRatio, heightRatio));
            }
        }

        private class ControlInfo
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public float FontSize { get; set; }

            public ControlInfo(int left, int top, int width, int height, float fontSize)
            {
                Left = left;
                Top = top;
                Width = width;
                Height = height;
                FontSize = fontSize;
            }
        }

        // Navigation Buttons for accessing different reports
        private void DailyReportbtn_Click(object sender, EventArgs e)
        {
            NavigateToForm(new DailyReport(_username));
        }

        private void MonthlyReportbtn_Click(object sender, EventArgs e)
        {
            NavigateToForm(new MonthlyReport(_username));
        }

        private void CustomerReportBtn_Click(object sender, EventArgs e)
        {
            NavigateToForm(new CustomerReport(_username));
        }

        private void CashierFormbtn_Click(object sender, EventArgs e)
        {
            NavigateToForm(new Cashier(_username));
        }

        private void signupbtn_Click(object sender, EventArgs e)
        {
            NavigateToForm(new SignUp(_username));
        }

        private void updateform_Click(object sender, EventArgs e)
        {
            NavigateToForm(new UserUpdate(_username));
        }

        private void changepassbtn_Click(object sender, EventArgs e)
        {
            NavigateToForm(new Changepass(_username));
        }

        private void NavigateToForm(Form form)
        {
            this.Hide();
            form.ShowDialog();
            this.Close();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
