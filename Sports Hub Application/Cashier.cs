using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Private_Pool_Application
{
    public partial class Cashier : KryptonForm
    {
        private string ConnectionString;
        private List<ComboBoxItem> allSports = new List<ComboBoxItem>();
        private string _username;




        private float _initialFormWidth;
        private float _initialFormHeight;
        private ControlInfo[] _controlsInfo;




        public Cashier(string username)
        {



            InitializeComponent();
            CustomInitializeComponent();
            ConnectionString = DatabaseConfig.connectionString;
            _username = username;

            mobilenumbertxt.MaxLength = 11; // Ensure no more than 11 characters

            mobilenumbertxt.KeyPress += mobilenumbertxt_KeyPress;
            ConfigureAutoComplete();

            // Configure autocomplete for nametxt
            ConfigureNameAutoComplete();

            // Attach the TextChanged event

            nametxt.TextChanged += new EventHandler(nametxt_TextChanged);



            Timer timer = new Timer
            {
                Interval = 1000 // 1 second
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            // Attach event handlers
            nametxt.TextChanged += nametxt_TextChanged;
            mobilenumbertxt.TextChanged += mobilenumbertxt_TextChanged;
            mobilenumbertxt.Leave += mobilenumbertxt_Leave;
            checknumbertxt.TextChanged += checknumbertxt_TextChanged;
            sportcombo.SelectedIndexChanged += sportcombo_SelectedIndexChanged_1;
            sportcombo.TextChanged += sportcombo_TextChanged;
            //membershiptxt.Leave += membershiptxt_Leave;
            membershiptxt.TextChanged += membershiptxt_TextChanged; // Optional
            pictureBox.Click += pictureBox_Click; // Handle PictureBox Click
            nametxt.Leave += nametxt_Leave;
            checknumbertxt.TextChanged += checknumbertxt_TextChanged_1;
            this.Load += Form1_Load;

            _initialFormWidth = this.Width;
            _initialFormHeight = this.Height;

            // Store initial size and location of all controls
            _controlsInfo = new ControlInfo[this.Controls.Count];
            for (int i = 0; i < this.Controls.Count; i++)
            {
                Control c = this.Controls[i];
                _controlsInfo[i] = new ControlInfo(c.Left, c.Top, c.Width, c.Height, c.Font.Size);
            }

            // Set event handler for form resize
            this.Resize += Home_Resize;




            categorycombo.SelectedIndexChanged += categorycombo_SelectedIndexChanged;
            sportcombo.SelectedIndexChanged += async (s, e) =>
            {
                if (categorycombo.SelectedItem != null)
                {
                    string selectedCategory = categorycombo.SelectedItem.ToString();
                    ComboBoxItem selectedSport = sportcombo.SelectedItem as ComboBoxItem;
                    if (selectedSport != null)
                    {
                        int sportID = selectedSport.Value;
                        await UpdateSportPriceAsync(sportID, selectedCategory);
                    }
                }
            };




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

                // Adjust font size
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

        private DateTime GetServerDateTime()
        {
            string connectionString = DatabaseConfig.connectionString;
            DateTime serverDateTime = DateTime.MinValue;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT GETDATE()";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    serverDateTime = (DateTime)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving server date and time: {ex.Message}");
                }
            }

            return serverDateTime;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime serverDateTime = GetServerDateTime();
            datetxt.Text = serverDateTime.ToString("yyyy-MM-dd");
            timetxt.Text = serverDateTime.ToString("HH:mm:ss");
        }

        private void CustomInitializeComponent()
        {
            // Custom initialization code goes here
        }

        private async void Form1_Load(object sender, EventArgs e)
        {



            SetButtonVisibilityBasedOnRole();



            await PopulateSportComboBoxAsync();
            ConfigureAutoComplete();

            cashiernamelabel.Text = _username;

            // if (_username == "control")
            //{
            //  btnExportImage.Visible = true;
            // }



        }

        private async Task PopulateSportComboBoxAsync()
        {
            allSports.Clear();
            sportcombo.Items.Clear();

            string query = "SELECT SportID, SportName FROM Sports";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int sportID = reader.GetInt32(0);
                                string sportName = reader.GetString(1);
                                ComboBoxItem item = new ComboBoxItem(sportID, sportName);
                                allSports.Add(item);
                                sportcombo.Items.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void ConfigureAutoComplete()
        {
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            string query = "SELECT MobileNumber FROM MixedGymDB.dbo.Users";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string mobileNumber = reader.GetString(0);
                                autoCompleteCollection.Add(mobileNumber);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }

            mobilenumbertxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mobilenumbertxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            mobilenumbertxt.AutoCompleteCustomSource = autoCompleteCollection;

            mobilenumbertxt.KeyDown += Mobilenumbertxt_KeyDown;
        }


        private async void Mobilenumbertxt_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Avoid processing when the suggestions are not visible
                if (mobilenumbertxt.AutoCompleteMode != AutoCompleteMode.None)
                {
                    e.SuppressKeyPress = true; // Prevent the default behavior of the Enter key
                    await SearchUserAsync(mobilenumbertxt.Text);
                }
            }
        }


        private async Task SearchUserAsync(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                ResetUserFields();
                return;
            }

            string query = "SELECT TOP 1 ID, Name, Category, ProfileImage FROM MixedGymDB.dbo.Users WHERE MobileNumber = @MobileNumber ORDER BY DateUpdated DESC";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MobileNumber", mobileNumber);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                membershiptxt.Text = reader.GetString(0);
                                nametxt.Text = reader.GetString(1);
                                categorycombo.Text = reader.GetString(2);

                                byte[] imageData = reader["ProfileImage"] as byte[];
                                if (imageData != null)
                                {
                                    try
                                    {
                                        using (MemoryStream ms = new MemoryStream(imageData))
                                        {
                                            // Save to a temporary file and reload
                                            string tempFilePath = Path.GetTempFileName();
                                            File.WriteAllBytes(tempFilePath, ms.ToArray());
                                            SetPictureBoxImage(Image.FromFile(tempFilePath));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Failed to load image: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        SetPictureBoxImage(null);
                                    }
                                }
                                else
                                {
                                    SetPictureBoxImage(null);
                                }

                                SetFieldsReadOnly(true);
                            }
                            else
                            {
                                ResetUserFields();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }




        private byte[] GetFileBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        private void ResetUserFields()
        {
            SetFieldsReadOnly(false);
            //pictureBox.Image = null; // Clear the image

        }

        private void SetFieldsReadOnly(bool readOnly)
        {
            nametxt.ReadOnly = readOnly;
            nametxt.Enabled = !readOnly;
            categorycombo.Enabled = !readOnly;
            mobilenumbertxt.ReadOnly = readOnly;
            mobilenumbertxt.Enabled = !readOnly;
            membershiptxt.ReadOnly = readOnly;
            membershiptxt.Enabled = !readOnly;

        }

        private void sportcombo_TextChanged(object sender, EventArgs e)
        {
            string filterText = sportcombo.Text.ToLower();
            sportcombo.Items.Clear();
            foreach (var sport in allSports.Where(s => s.Text.ToLower().Contains(filterText)))
            {
                sportcombo.Items.Add(sport);
            }
            sportcombo.DroppedDown = true;
            sportcombo.SelectionStart = filterText.Length;
            sportcombo.SelectionLength = 0;
            Cursor.Current = Cursors.Default;
        }

        private async void sportcombo_SelectedIndexChanged_1(object sender, EventArgs e)
        {


            ComboBoxItem selectedSport = sportcombo.SelectedItem as ComboBoxItem;
            if (selectedSport != null && categorycombo.SelectedItem != null)
            {
                int sportID = selectedSport.Value;
                string selectedCategory = categorycombo.SelectedItem.ToString();
                await UpdateSportPriceAsync(sportID, selectedCategory);
            }
        }




        private async Task UpdateSportPriceAsync(int sportID, string category)
        {
            string priceColumn;

            // Use if-else instead of switch expression
            if (category == "عضو")
            {
                priceColumn = "MemberPrice";
            }
            else if (category == "مدني")
            {
                priceColumn = "CivilianPrice";
            }
            else if (category == "درجة 1")
            {
                priceColumn = "Degree1Price";
            }
            else if (category == "جيش")
            {
                priceColumn = "MilitaryPrice";
            }
            else
            {
                throw new ArgumentException("Invalid category");
            }

            string query = $"SELECT {priceColumn} FROM Sports WHERE SportID = @SportID";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SportID", sportID);
                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        if (result != null && decimal.TryParse(result.ToString(), out decimal price))
                        {
                            // Ensure the label update is done on the UI thread
                            if (sportpricelistlabel.InvokeRequired)
                            {
                                sportpricelistlabel.Invoke(new Action(() =>
                                {
                                    sportpricelistlabel.Text = "Total price = " + price.ToString("F2");
                                }));
                            }
                            else
                            {
                                sportpricelistlabel.Text = "Total price = " + price.ToString("F2");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Price not found or invalid.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }


        private async Task<bool> CheckIfCheckNumberExistsAsync(string checkNumber)
        {
            string query = "SELECT COUNT(*) FROM Transactions WHERE CheckNumber = @CheckNumber";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CheckNumber", checkNumber);

                    try
                    {
                        await connection.OpenAsync();
                        int count = (int)await command.ExecuteScalarAsync();
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        private async Task<(string UserName, int UserID)> CheckIfMobileNumberExistsAsync(string mobileNumber)
        {
            string query = "SELECT Name, UserID FROM MixedGymDB.dbo.Users WHERE MobileNumber = @MobileNumber";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MobileNumber", mobileNumber);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return (reader["Name"].ToString(), Convert.ToInt32(reader["UserID"]));
                            }
                            return (null, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                        return (null, 0);
                    }
                }
            }
        }












        private async void addCustomerButton_Click_1(object sender, EventArgs e)
        {
            string name = nametxt.Text;
            string checkNumber = checknumbertxt.Text;
            string mobileNumber = mobilenumbertxt.Text;
            string category = categorycombo.Text;
            string id = membershiptxt.Text;
            string notes = notestxt.Text;
            Image profileImage = pictureBox.Image; // Get image from pictureBox
            byte[] pdfFile = null;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(mobileNumber) || string.IsNullOrWhiteSpace(checkNumber) || string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("برجاء ملي كافه البيانات");
                return;
            }

            if (await CheckIfCheckNumberExistsAsync(checkNumber))
            {
                MessageBox.Show("This check number is already used. Please provide a different check number.");
                return;
            }

            var (existingUserName, existingUserID) = await CheckIfMobileNumberExistsAsync(mobileNumber);
            if (!string.IsNullOrEmpty(existingUserName) && existingUserName != name)
            {
                MessageBox.Show($"This mobile number is already used for {existingUserName}. Please provide a different mobile number.");
                return;
            }

            if (mobilenumbertxt.Text.Length != 11)
            {
                MessageBox.Show("برجاء ادخال رقم تليفون صحيح");
                return;
            }

            if (pictureBox.Image == null)
            {
                MessageBox.Show("برجاء ادخال صوره للعميل");
                return;
            }

            ComboBoxItem selectedSport = null;
            string enteredSport = sportcombo.Text;
            bool sportMatchFound = false;

            if (!string.IsNullOrWhiteSpace(enteredSport))
            {
                // Check if the entered value matches any item in the ComboBox
                foreach (ComboBoxItem item in sportcombo.Items)
                {
                    if (item.Text.Equals(enteredSport, StringComparison.OrdinalIgnoreCase))
                    {
                        sportMatchFound = true;
                        selectedSport = item;
                        sportcombo.SelectedItem = item;
                        break;
                    }
                }
            }

            if (!sportMatchFound)
            {
                MessageBox.Show("Invalid sport type. Please select a valid sport from the list.");
                return;
            }

            if (!decimal.TryParse(paidtxt.Text, out decimal amountPaid))
            {
                MessageBox.Show("Invalid amount paid.");
                return;
            }

            // Define the connection strings for user and transaction databases
            string usersConnectionString = DatabaseConfig.connectionString;
            string transactionsConnectionString = DatabaseConfig.connectionString;

            try
            {
                // Step 1: Save the user and get the userID
                int userID;

                using (SqlConnection usersConnection = new SqlConnection(usersConnectionString))
                {
                    await usersConnection.OpenAsync();
                    userID = await SaveUserAsync(name, mobileNumber, category, id, profileImage, pdfFile, usersConnection);
                }

                // Step 2: Save the transaction with notes
                if (selectedSport != null)
                {
                    using (SqlConnection transactionsConnection = new SqlConnection(transactionsConnectionString))
                    {
                        await transactionsConnection.OpenAsync();
                        await SaveTransactionAsync(userID, selectedSport.Value, amountPaid, discountPercentage, checkNumber, notes, transactionsConnection);
                    }
                }

                // Define folder path
               string folderPath = @"\\192.168.50.5\e\Daily Backup\PrivatePool ScreenShots";
               // string folderPath = @"F:\New folder";

                // Create directory if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Capture screenshot of the form
                using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
                {
                    this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                    string filePath = GetUniqueFilePath(folderPath, name, "png");
                    bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                MessageBox.Show("تم اضافه العميل");

                // Open the Cashier form and close the current form
                this.Hide();
                Cashier CashierForm = new Cashier(_username);
                CashierForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Helper method to generate a unique file path for screenshots
        private string GetUniqueFilePath(string folderPath, string baseName, string extension)
        {
            string fileName = $"{baseName}_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
            return Path.Combine(folderPath, fileName);
        }



        // Method to generate a unique file path



        private async Task<DateTime> GetServerDateTimeAsync(SqlConnection connection)
        {
            string query = "SELECT GETDATE()";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    return (DateTime)await command.ExecuteScalarAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving server date and time: {ex.Message}");
                    throw;
                }
            }
        }




        private async Task<int> SaveUserAsync(
      string name,
      string mobileNumber,
      string category,
      string id,
      Image profileImage,
      byte[] pdfFile,
      SqlConnection usersConnection)
        {
            string checkUserQuery = @"
SELECT UserID, Name, MobileNumber, Category, ID, ProfileImage
FROM MixedGymDB.dbo.Users 
WHERE Name = @Name 
  AND MobileNumber = @MobileNumber 
  AND Category = @Category 
  AND ID = @ID 
  AND ProfileImage = @ProfileImage";


            using (SqlCommand checkCommand = new SqlCommand(checkUserQuery, usersConnection))
            {
                checkCommand.Parameters.AddWithValue("@Name", name);
                checkCommand.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                checkCommand.Parameters.AddWithValue("@Category", category);
                checkCommand.Parameters.AddWithValue("@ID", id);

                if (profileImage != null)
                {
                    checkCommand.Parameters.Add("@ProfileImage", SqlDbType.VarBinary, -1).Value = GetImageBytes(profileImage);
                }
                else
                {
                    checkCommand.Parameters.Add("@ProfileImage", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                }

                using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        byte[] existingProfileImage = reader.IsDBNull(5) ? null : (byte[])reader[5];

                        if (name != reader.GetString(1) || mobileNumber != reader.GetString(2) ||
                            category != reader.GetString(3) || id != reader.GetString(4) ||
                            !Equals(GetImageBytes(profileImage), existingProfileImage))
                        {
                            return await InsertNewUserAsync(name, mobileNumber, category, id, profileImage, pdfFile, usersConnection);
                        }
                        else
                        {
                            return userId;
                        }
                    }
                }
            }

            return await InsertNewUserAsync(name, mobileNumber, category, id, profileImage, pdfFile, usersConnection);
        }



        private async Task<int> InsertNewUserAsync(
       string name,
       string mobileNumber,
       string category,
       string id,
       Image profileImage,
       byte[] pdfFile,
       SqlConnection usersConnection)
        {
            string insertQuery = @"
INSERT INTO MixedGymDB.dbo.Users (Name, MobileNumber, Category, ID, ProfileImage, DateUpdated) 
OUTPUT INSERTED.UserID 
VALUES (@Name, @MobileNumber, @Category, @ID, @ProfileImage, @DateUpdated)";


            using (SqlCommand command = new SqlCommand(insertQuery, usersConnection))
            {
                DateTime serverDateTime = await GetServerDateTimeAsync(usersConnection);

                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@DateUpdated", serverDateTime);

                if (profileImage != null)
                {
                    command.Parameters.Add("@ProfileImage", SqlDbType.VarBinary, -1).Value = GetImageBytes(profileImage);
                }
                else
                {
                    command.Parameters.Add("@ProfileImage", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                }

                if (pdfFile != null && pdfFile.Length > 0)
                {
                    command.Parameters.Add("@PDFFile", SqlDbType.VarBinary, -1).Value = pdfFile;
                }
                else
                {
                    command.Parameters.Add("@PDFFile", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                }

                return (int)await command.ExecuteScalarAsync();
            }
        }



        // Helper method to convert Image to byte[]
        private byte[] GetImageBytes(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        // Helper method to compare byte arrays
        private bool Equals(byte[] a, byte[] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }




        // Helper method to convert Image to byte array
        //private byte[] GetImageBytes(Image image)
        //{
        //using (MemoryStream ms = new MemoryStream())
        // {
        //image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        // return ms.ToArray();
        //}
        //}




        // Your applyDiscountButton_Click method should set this value


        private void ResetDiscount()
        {
            discountPercentage = 0;
            sportpricelistlabel.Text = "Total price = " + originalTotalPrice.ToString("0.00");
            isDiscountApplied = false;
        }


        // Save the transaction with the discount percentage
        private async Task SaveTransactionAsync(int userID, int sportID, decimal amountPaid, decimal discountPercentage, string checkNumber, string notes, SqlConnection transactionsConnection)
        {
            string storedProcedureName = "InsertTransaction";

            using (SqlCommand command = new SqlCommand(storedProcedureName, transactionsConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                DateTime serverDateTime = await GetServerDateTimeAsync(transactionsConnection);

                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@SportID", sportID);
                command.Parameters.AddWithValue("@AmountPaid", amountPaid);
                command.Parameters.AddWithValue("@DateAndTime", serverDateTime);
                command.Parameters.AddWithValue("@CashierName", _username);
                command.Parameters.AddWithValue("@DiscountPercentage", discountPercentage);
                command.Parameters.AddWithValue("@notes", notes);
                command.Parameters.AddWithValue("@CheckNumber", checkNumber);

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the transaction: " + ex.Message);
                }
            }
        }










        private async Task<int?> GetExistingUserIDAsync(string name,
            string mobileNumber, string category, string id, SqlConnection connection)
        {
            string query = @"
SELECT UserID 
FROM MixedGymDB.dbo.Users 
WHERE Name = @Name 
  AND MobileNumber = @MobileNumber 
  AND Category = @Category 
  AND ID = @ID";


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@ID", id);

                object result = await command.ExecuteScalarAsync();
                return result != null ? (int)result : (int?)null;
            }
        }
        private async Task SearchUserByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ResetUserFields();
                return;
            }

            // Normalize the input name
            string normalizedInput = NormalizeArabicText(name);
            Debug.WriteLine($"Normalized Input: {normalizedInput}");

            string query = @"
SELECT ID, MobileNumber, Name, Category, ProfileImage 
FROM MixedGymDB.dbo.Users 
WHERE dbo.NormalizeArabicText(Name) LIKE '%' + dbo.NormalizeArabicText(@Name) + '%'
ORDER BY DateUpdated DESC";


            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Pass the normalized input to the query
                    command.Parameters.AddWithValue("@Name", normalizedInput);

                    Debug.WriteLine($"Query: {query}");
                    Debug.WriteLine($"Parameter @Name: {normalizedInput}");

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string id = reader["ID"]?.ToString() ?? "N/A";
                                string mobileNumber = reader["Mobilenumber"]?.ToString() ?? "N/A";
                                string names = reader["Name"]?.ToString() ?? "N/A";
                                string category = reader["Category"]?.ToString() ?? "N/A";

                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        membershiptxt.Text = id;
                                        mobilenumbertxt.Text = mobileNumber;
                                        nametxt.Text = names;
                                        categorycombo.Text = category;
                                    }));
                                }
                                else
                                {
                                    membershiptxt.Text = id;
                                    mobilenumbertxt.Text = mobileNumber;
                                    nametxt.Text = names;
                                    categorycombo.Text = category;
                                }

                                byte[] imageData = reader["ProfileImage"] as byte[];
                                if (imageData != null)
                                {
                                    try
                                    {
                                        using (MemoryStream ms = new MemoryStream(imageData))
                                        {
                                            string tempFilePath = Path.GetTempFileName();
                                            File.WriteAllBytes(tempFilePath, ms.ToArray());
                                            SetPictureBoxImage(Image.FromFile(tempFilePath));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Failed to load image: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        SetPictureBoxImage(null);
                                    }
                                }
                                else
                                {
                                    SetPictureBoxImage(null);
                                }

                                SetFieldsReadOnly(true);
                            }
                            else
                            {
                                Debug.WriteLine("No user found with the given name.");
                                ResetUserFields();
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("A SQL error occurred: " + ex.Message);
                        Debug.WriteLine($"SQL Exception: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                        Debug.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }
        }






        // Event handler for nametxt TextChanged event
        private async void nametxt_TextChanged(object sender, EventArgs e)
        {


        }
        private async void nametxt_Leave(object sender, EventArgs e)
        {

            await SearchUserByNameAsync(nametxt.Text);

        }


        private string NormalizeArabicText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            return text
                .Replace('أ', 'ا')  // Normalize 'أ' to 'ا'
                .Replace('إ', 'ا')  // Normalize 'إ' to 'ا'
                .Replace('آ', 'ا')  // Normalize 'آ' to 'ا'
                .Replace('ى', 'ي')  // Normalize 'ى' to 'ي'
              
             
                .Replace('ؤ', 'و'); // Normalize 'ؤ' to 'و'
        }


        private string ReverseNormalizeArabicText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text; // Return input as is if it's null or empty

            // Perform reverse normalization
            return text
                .Replace('ا', 'أ')  // Reverse normalize 'ا' to 'أ'
                .Replace('ا', 'إ')  // Reverse normalize 'ا' to 'إ'
                .Replace('ا', 'آ')  // Reverse normalize 'ا' to 'آ'
                .Replace('ي', 'ى')  // Reverse normalize 'ي' to 'ى'
           
             
                .Replace('و', 'ؤ'); // Reverse normalize 'و' to 'ؤ'
        }







        private void ConfigureNameAutoComplete()
        {
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            string query = "SELECT Name FROM MixedGymDB.dbo.Users";


            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString(0);
                                // Add both normalized and reverse-normalized versions to the collection
                                autoCompleteCollection.Add(NormalizeArabicText(name));
                                autoCompleteCollection.Add(ReverseNormalizeArabicText(name));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }

            nametxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            nametxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            nametxt.AutoCompleteCustomSource = autoCompleteCollection;

            nametxt.KeyDown += Nametxt_KeyDown;
        }














        private async void Nametxt_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Avoid processing when the suggestions are not visible
                if (nametxt.AutoCompleteMode != AutoCompleteMode.None)
                {
                    e.SuppressKeyPress = true; // Prevent the default behavior of the Enter key
                    await SearchUserByNameAsync(nametxt.Text);
                }
            }
        }
        private async void Nametxt_TextChanged(object sender, EventArgs e)
        {
            if (nametxt.AutoCompleteMode == AutoCompleteMode.None)
            {
                await SearchUserByNameAsync(nametxt.Text);
            }
        }





        private void mobilenumbertxt_TextChanged(object sender, EventArgs e)
        {
            // Optional: Handle phone number text changes if needed
        }

        private async void mobilenumbertxt_Leave(object sender, EventArgs e)
        {
            await SearchUserAsync(mobilenumbertxt.Text);
        }

        private void checknumbertxt_TextChanged(object sender, EventArgs e)
        {
            // Optional: Handle check number text changes if needed
        }

        private void serviceButton_Click_1(object sender, EventArgs e)
        {
            ResetForm();
            UpdateCheckNumber();
        }

        private void ResetForm()
        {
            sportcombo.SelectedIndexChanged -= sportcombo_SelectedIndexChanged_1;
            categorycombo.SelectedIndexChanged -= sportcombo_SelectedIndexChanged_1;

            nametxt.Clear();
            checknumbertxt.Clear();
            mobilenumbertxt.Clear();
            membershiptxt.Clear();
            paidtxt.Clear();
            sportpricelistlabel.Text = "";
            notestxt.Clear();

            sportcombo.SelectedIndex = -1;
            categorycombo.SelectedIndex = -1;
            pictureBox.Image = null; // Clear the image

            SetFieldsReadOnly(false);

            nametxt.Focus();
            sportcombo.DroppedDown = false;
            categorycombo.DroppedDown = false;

            sportcombo.SelectedIndexChanged += sportcombo_SelectedIndexChanged_1;
            categorycombo.SelectedIndexChanged += sportcombo_SelectedIndexChanged_1;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            // {
            //  pictureBox.Image = Image.FromFile(openFileDialog1.FileName);
            //}
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            // Handle back button click if needed
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void mobilenumbertxt_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void serviceButton2_Click(object sender, EventArgs e)
        {
            ResettForm();
            //MembershipSetFieldsReadOnly(false);
            SetFieldsReadOnly(false);
        }

        private void ResettForm()
        {
            sportcombo.SelectedIndexChanged -= sportcombo_SelectedIndexChanged_1;
            categorycombo.SelectedIndexChanged -= sportcombo_SelectedIndexChanged_1;

            nametxt.Clear();
            checknumbertxt.Clear();
            mobilenumbertxt.Clear();
            membershiptxt.Clear();
            paidtxt.Clear();
            sportpricelistlabel.Text = "";
            notestxt.Clear();

            sportcombo.SelectedIndex = -1;
            categorycombo.SelectedIndex = -1;
            pictureBox.Image = null; // Clear the image

            SetFieldsReadOnly(false);

            nametxt.Focus();
            sportcombo.DroppedDown = false;
            categorycombo.DroppedDown = false;

            sportcombo.SelectedIndexChanged += sportcombo_SelectedIndexChanged_1;
            categorycombo.SelectedIndexChanged += sportcombo_SelectedIndexChanged_1;
        }

        private void backButton_Click_2(object sender, EventArgs e)
        {
            Home loginForm = new Home("");
            this.Close();
            loginForm.Show();
        }

        private void cashiernamelabel_Click(object sender, EventArgs e)
        {

        }

        private void btnImport_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|PDF Files (*.pdf)|*.pdf";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    string fileExtension = Path.GetExtension(selectedFile).ToLower();

                    if (fileExtension == ".pdf")
                    {
                        DisplayPdf(selectedFile);
                    }
                    else if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
                    {
                        DisplayImage(selectedFile);
                    }
                    else
                    {
                        MessageBox.Show("Unsupported file type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DisplayImage(string filePath)
        {
            pictureBox.Image = Image.FromFile(filePath);
        }

        private void DisplayPdf(string filePath)
        {
            using (var document = PdfiumViewer.PdfDocument.Load(filePath))
            {
                var page = document.Render(0, pictureBox.Width, pictureBox.Height, true);
                pictureBox.Image = page;
            }
        }

        /* private async Task SearchByMembershipIDAsync(string id)
         {
             if (string.IsNullOrWhiteSpace(id))
             {
                 MembershipResetUserFields();
                 return;
             }

             string query = "SELECT TOP 1 MobileNumber, Name, Category, ProfileImage FROM Users WHERE ID = @ID ORDER BY DateUpdated DESC";

             using (SqlConnection connection = new SqlConnection(ConnectionString))
             {
                 using (SqlCommand command = new SqlCommand(query, connection))
                 {
                     command.Parameters.AddWithValue("@ID", id);

                     try
                     {
                         await connection.OpenAsync();
                         using (SqlDataReader reader = await command.ExecuteReaderAsync())
                         {
                             if (await reader.ReadAsync())
                             {
                                 mobilenumbertxt.Text = reader.GetString(0);
                                 nametxt.Text = reader.GetString(1);
                                 categorycombo.Text = reader.GetString(2);

                                 byte[] imageData = reader["ProfileImage"] as byte[];
                                 if (imageData != null)
                                 {
                                     try
                                     {
                                         using (MemoryStream ms = new MemoryStream(imageData))
                                         {
                                             // Save to a temporary file and reload
                                             string tempFilePath = Path.GetTempFileName();
                                             File.WriteAllBytes(tempFilePath, ms.ToArray());
                                             pictureBox.Image = Image.FromFile(tempFilePath);
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         MessageBox.Show("Failed to load image: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                         pictureBox.Image = null;
                                     }
                                 }
                                 else
                                 {
                                     pictureBox.Image = null;
                                 }

                                 //MembershipSetFieldsReadOnly(true);
                             }
                             else
                             {
                                 MembershipResetUserFields();
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show("An error occurred: " + ex.Message);
                     }
                 }
             }
         }*/






        private void SetPictureBoxImage(Image newImage)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
            pictureBox.Image = newImage;
        }






        //private void MembershipResetUserFields()
        //  {
        //    MembershipSetFieldsReadOnly(false);

        //
        // }

        // private void MembershipSetFieldsReadOnly(bool readOnly)
        // {
        //nametxt.ReadOnly = readOnly;
        //    categorycombo.Enabled = !readOnly;



        // }


        /*private async void membershiptxt_Leave(object sender, EventArgs e)
        {
            await SearchByMembershipIDAsync(membershiptxt.Text);
        }*/


        private void membershiptxt_TextChanged(object sender, EventArgs e)
        {

        }




        private void btnExportImage_Click(object sender, EventArgs e)
        {
            // Check if there is an image in the PictureBox
            if (pictureBox.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Set the default file name and filter
                    saveFileDialog.FileName = "exported_image.png";
                    saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                    saveFileDialog.Title = "Save Image As";

                    // Show the SaveFileDialog and check if the user selected a file
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file path
                        string filePath = saveFileDialog.FileName;

                        // Save the image in the selected format
                        ImageFormat format = ImageFormat.Png;
                        switch (Path.GetExtension(filePath).ToLower())
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = ImageFormat.Jpeg;
                                break;
                            case ".bmp":
                                format = ImageFormat.Bmp;
                                break;
                        }

                        // Save the image
                        pictureBox.Image.Save(filePath, format);
                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("No image to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mobilenumbertxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (backspace, enter, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the character is not a control character or a digit, block it
                e.Handled = true;
            }
        }

        private void clearimagebtn_Click_1(object sender, EventArgs e)
        {
            pictureBox.Image = null;
        }






        private bool isDiscountApplied = false; // Flag to track if discount is already applied
        private decimal originalTotalPrice; // Variable to store the original total price
        private decimal discountPercentage; // Variable to store the discount percentage

        private void applyDiscountButton_Click_1(object sender, EventArgs e)
        {
            decimal totalPrice;

            if (!decimal.TryParse(sportpricelistlabel.Text.Split('=')[1].Trim(), out totalPrice))
            {
                MessageBox.Show("Invalid total price.");
                return;
            }

            // Prompt for discount percentage
            string discountInput = Microsoft.VisualBasic.Interaction.InputBox("Enter discount percentage (e.g., 20 for 20%):", "Apply Discount", "0");
            if (!decimal.TryParse(discountInput, out discountPercentage) || discountPercentage < 0 || discountPercentage > 100)
            {
                MessageBox.Show("Invalid discount percentage.");
                return;
            }

            if (isDiscountApplied)
            {
                // Reset the total price to the original value before applying new discount
                totalPrice = originalTotalPrice;
            }
            else
            {
                // Store the original total price before applying the first discount
                originalTotalPrice = totalPrice;
            }

            // Calculate the discount amount and the new total price
            decimal discountAmount = (totalPrice * discountPercentage) / 100;
            decimal discountedTotal = totalPrice - discountAmount;

            // Update the total price label
            sportpricelistlabel.Text = "Total price = " + discountedTotal.ToString("0.00");

            MessageBox.Show($"Discount applied. New total price: {discountedTotal:0.00}");

            isDiscountApplied = true; // Set the flag to true indicating the discount has been applied
        }



        public class ComboBoxItem
        {
            public int Value { get; }
            public string Text { get; }

            public ComboBoxItem(int value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
            private void Form1_FormClosed(object sender, FormClosedEventArgs e)
            {
                // Ensure the application exits when this form is closed
                Application.Exit();
            }
        }

        private void categorycombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void notestxt_TextChanged(object sender, EventArgs e)
        {

        }



        private void backbtn_Click(object sender, EventArgs e)
        {
            // Assuming you have a method to get the current username
            this.Hide();
            Home homeform = new Home(_username);

            homeform.ShowDialog();
            this.Close();
            ;
            // Fetch the role ID for the current user from the database

        }


        private void SetButtonVisibilityBasedOnRole()
        {
            int roleID = GetRoleIdForCurrentUser();

            if (roleID == 1)
            {
                // Hide the button if role ID is 1
                backbtn.Visible = false;
                exportimgbtn.Visible = false;
                applyDiscountButton.Visible = false;


            }
            else if (roleID == 2)
            {
                // Show the button if role ID is 2
                backbtn.Visible = false;
                exportimgbtn.Visible = false;
                applyDiscountButton.Visible = true;

            }
            else if (roleID == 3)
            {
                // Show the button if role ID is 2
                backbtn.Visible = true;
                exportimgbtn.Visible = false;
                applyDiscountButton.Visible = true;

            }
            else if (roleID == 4)
            {
                // Show the button if role ID is 2
                backbtn.Visible = true;
                exportimgbtn.Visible = true;
                applyDiscountButton.Visible = true;

            }
        }

        private int GetRoleIdForCurrentUser()
        {
            int roleID = 0;
            string username = Login.LoggedInUsername; // Assuming this is how you store the logged-in username

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                string query = @"
               SELECT RoleID
            FROM MixedGymDB.dbo.CashierDetails
            WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out roleID))
                        {
                            return roleID;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }

            return roleID;
        }

        private void exportimgbtn_Click(object sender, EventArgs e)
        {
            // Check if there is an image in the PictureBox
            if (pictureBox.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Set the default file name and filter
                    saveFileDialog.FileName = "exported_image.png";
                    saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                    saveFileDialog.Title = "Save Image As";

                    // Show the SaveFileDialog and check if the user selected a file
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file path
                        string filePath = saveFileDialog.FileName;

                        // Save the image in the selected format
                        ImageFormat format = ImageFormat.Png;
                        switch (Path.GetExtension(filePath).ToLower())
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = ImageFormat.Jpeg;
                                break;
                            case ".bmp":
                                format = ImageFormat.Bmp;
                                break;
                        }

                        // Save the image
                        pictureBox.Image.Save(filePath, format);
                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("No image to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void categorycombo_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        private void UpdateCheckNumber()
        {


            using (SqlConnection connection = new SqlConnection(DatabaseConfig.connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                SELECT TOP 1 CheckNumber
                FROM Transactions
                ORDER BY Transactionid DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();


                        if (result != null && int.TryParse(result.ToString(), out int lastCheckNumber))
                        {
                            int newCheckNumber = lastCheckNumber + 1;
                            checknumbertxt.Text = newCheckNumber.ToString();

                        }
                        else
                        {
                            checknumbertxt.Text = "1";

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        private void Cashier_Load(object sender, EventArgs e)
        {
            UpdateCheckNumber();
        }



        private async void categorycombo_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            if (sportcombo.SelectedItem is ComboBoxItem selectedSport && categorycombo.SelectedItem != null)
            {
                string selectedCategory = categorycombo.SelectedItem.ToString();
                int sportID = selectedSport.Value;
                await UpdateSportPriceAsync(sportID, selectedCategory);
            }
        }

        private void nametxt_TextChanged_1(object sender, EventArgs e)
        {

        }


        private void mobilenumbertxt_TextChanged_1(object sender, EventArgs e)
        {
            if (mobilenumbertxt.Text.Length != 11 && mobilenumbertxt.Text.Length != 0)
            {
                mobilenumbertxt.BackColor = Color.LightCoral; // Change color to indicate error
            }
            else
            {
                mobilenumbertxt.BackColor = Color.White; // Reset to default color
            }
        }



        private void cashiernamelabl_Click(object sender, EventArgs e)
        {

        }

        private void cashiernamelabel_Click_1(object sender, EventArgs e)
        {

        }

        private void CashierReport_Click(object sender, EventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            CashierDailyReport cashierDailyReport = new CashierDailyReport(_username);
            cashierDailyReport.ShowDialog();
            this.Close();
        }

        private async void updateuserbtn_Click(object sender, EventArgs e)
        {
            string phoneNumber = PromptForPhoneNumber();
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Please enter a valid phone number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await GetAndDisplayLastTransactionAsync(phoneNumber);
        }

        private string PromptForPhoneNumber()
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 500;
                prompt.Height = 150;
                prompt.Text = "Enter Phone Number";

                Label textLabel = new Label() { Left = 50, Top = 20, Text = "Phone Number:" };
                TextBox textBox = new TextBox() { Left = 150, Top = 20, Width = 200 };

                Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 20 };
                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                prompt.ShowDialog();
                return textBox.Text;
            }
        }

        private async Task GetAndDisplayLastTransactionAsync(string phoneNumber)
        {
            string query = @"
        SELECT TOP 1 
            t.TransactionID, 
            t.UserID, 
            t.SportID, 
            t.AmountPaid, 
            t.RemainingAmount, 
            t.DateAndTime, 
            t.CashierName, 
            t.DiscountPercentage, 
            t.Notes
        FROM Transactions t
        JOIN MixedGymDB.dbo.Users u ON t.UserID = u.UserID
        WHERE u.MobileNumber = @MobileNumber
        ORDER BY t.DateAndTime DESC";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MobileNumber", phoneNumber);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Transaction transaction = new Transaction
                                {
                                    TransactionID = reader.GetInt32(0),
                                    UserID = reader.GetInt32(1),
                                    SportID = reader.GetInt32(2),
                                    AmountPaid = reader.GetDecimal(3),
                                    RemainingAmount = reader.GetDecimal(4),
                                    DateAndTime = reader.GetDateTime(5),
                                    CashierName = reader.GetString(6),
                                    DiscountPercentage = reader.GetDecimal(7),
                                    Notes = reader.GetString(8)
                                };

                                ShowEditTransactionForm(transaction);
                            }
                            else
                            {
                                MessageBox.Show("No transactions found for the entered phone number.", "No Transactions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowEditTransactionForm(Transaction transaction)
        {
            Form editForm = new Form
            {
                Width = 600,
                Height = 400,
                Text = "Edit Transaction"
            };

            TextBox amountPaidTextBox = new TextBox { Left = 150, Top = 20, Width = 200, Text = transaction.AmountPaid.ToString() };
            TextBox remainingAmountTextBox = new TextBox { Left = 150, Top = 60, Width = 200, Text = transaction.RemainingAmount.ToString() };

            TextBox discountPercentageTextBox = new TextBox { Left = 150, Top = 100, Width = 200, Text = transaction.DiscountPercentage.ToString() };
            TextBox notesTextBox = new TextBox { Left = 150, Top = 140, Width = 200, Text = transaction.Notes };

            Button saveButton = new Button { Text = "Save", Left = 150, Width = 100, Top = 220 };
            saveButton.Click += async (sender, e) =>
            {
                transaction.AmountPaid = decimal.Parse(amountPaidTextBox.Text);
                transaction.RemainingAmount = decimal.Parse(remainingAmountTextBox.Text);

                transaction.DiscountPercentage = decimal.Parse(discountPercentageTextBox.Text);
                transaction.Notes = notesTextBox.Text;

                await UpdateTransactionAsync(transaction);
                editForm.Close();
            };

            editForm.Controls.Add(new Label { Left = 50, Top = 20, Text = "Amount Paid:" });
            editForm.Controls.Add(amountPaidTextBox);
            editForm.Controls.Add(new Label { Left = 50, Top = 60, Text = "Remaining Amount:" });
            editForm.Controls.Add(remainingAmountTextBox);


            editForm.Controls.Add(new Label { Left = 50, Top = 100, Text = "Discount Percentage:" });
            editForm.Controls.Add(discountPercentageTextBox);
            editForm.Controls.Add(new Label { Left = 50, Top = 140, Text = "Notes:" });
            editForm.Controls.Add(notesTextBox);
            editForm.Controls.Add(saveButton);

            editForm.ShowDialog();
        }

        private async Task UpdateTransactionAsync(Transaction transaction)
        {
            string query = @"
        UPDATE Transactions
        SET 
            AmountPaid = @AmountPaid, 
            RemainingAmount = @RemainingAmount, 
            CashierName = @CashierName, 
            DiscountPercentage = @DiscountPercentage, 
            Notes = @Notes
        WHERE TransactionID = @TransactionID";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AmountPaid", transaction.AmountPaid);
                    command.Parameters.AddWithValue("@RemainingAmount", transaction.RemainingAmount);

                    command.Parameters.AddWithValue("@DiscountPercentage", transaction.DiscountPercentage);
                    command.Parameters.AddWithValue("@Notes", transaction.Notes);
                    command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        MessageBox.Show("Transaction updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public class Transaction
        {
            public int TransactionID { get; set; }
            public int UserID { get; set; }
            public int SportID { get; set; }
            public decimal AmountPaid { get; set; }
            public decimal RemainingAmount { get; set; }
            public DateTime DateAndTime { get; set; }
            public string CashierName { get; set; }
            public decimal DiscountPercentage { get; set; }
            public string Notes { get; set; }
        }

        private void checknumbertxt_TextChanged_1(object sender, EventArgs e)
        {

        }





        private async Task<decimal> GetSportPriceAsync(int sportID, string category)
        {
            string priceColumn;

            // Determine the correct price column based on the category
            if (category == "عضو")
            {
                priceColumn = "MemberPrice";
            }
            else if (category == "مدني")
            {
                priceColumn = "CivilianPrice";
            }
            else if (category == "درجة 1")
            {
                priceColumn = "Degree1Price";
            }
            else if (category == "جيش")
            {
                priceColumn = "MilitaryPrice";
            }
            else
            {
                throw new ArgumentException("Invalid category");
            }

            string query = $"SELECT {priceColumn} FROM Sports WHERE SportID = @SportID";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SportID", sportID);

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal price))
                        {
                            return price;
                        }
                        else
                        {
                            throw new InvalidOperationException("Price not found or invalid.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("An error occurred while retrieving the sport price.", ex);
                    }
                }
            }
        }





        private async void Swimmingbtn_Click(object sender, EventArgs e)
        {
            // Set the sport combo box text to "دخول حمام سباحه"
            sportcombo.Text = "دخول حمام سباحه";

            // Hardcode the sport ID to 33 for swimming
            int sportID = 33;

            // Check if a category is selected
            if (string.IsNullOrWhiteSpace(categorycombo.SelectedItem?.ToString()))
            {
                // Show a message box if no category is selected
                MessageBox.Show("يرجى اختيار فئة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method if no category is selected
            }

            // Get the selected category
            string selectedCategory = categorycombo.SelectedItem.ToString();

            // Update the sport price based on the selected category
            await UpdateSportPriceAsync(sportID, selectedCategory);

            // Update the price display or any other UI elements as needed
        }


        private async void lockerbtn_Click(object sender, EventArgs e)
        {
            // Set the sport combo box text to "دخول حمام سباحه"
            sportcombo.Text = "لوكر";

            // Hardcode the sport ID to 33 for swimming
            int sportID = 32;

            // Ensure the category is selected
            if (string.IsNullOrWhiteSpace(categorycombo.SelectedItem?.ToString()))
            {
                // Show a message box if no category is selected
                MessageBox.Show("يرجى اختيار فئة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method if no category is selected
            }

            string selectedCategory = categorycombo.SelectedItem.ToString();
            await UpdateSportPriceAsync(sportID, selectedCategory);
        }

        private async void kryptonButton4_Click(object sender, EventArgs e)
        {
            sportcombo.Text = "دخول حمام سباحه + لوكر";


            // Hardcode the sport ID to 33 for swimming
            int sportID = 34;
            if (string.IsNullOrWhiteSpace(categorycombo.SelectedItem?.ToString()))
            {
                // Show a message box if no category is selected
                MessageBox.Show("يرجى اختيار فئة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method if no category is selected
            }
            // Ensure the category is selected


            string selectedCategory = categorycombo.SelectedItem.ToString();
            await UpdateSportPriceAsync(sportID, selectedCategory);
        }
    }
}









