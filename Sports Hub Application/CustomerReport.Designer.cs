using System.Windows.Forms;

namespace Private_Pool_Application
{
    partial class CustomerReport
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox nametxt;
        private DataGridView transactionsGridView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerReport));
            this.nametxt = new System.Windows.Forms.TextBox();
            this.transactionsGridView = new System.Windows.Forms.DataGridView();
            this.nameLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Categorytxt = new System.Windows.Forms.TextBox();
            this.membershipIDtxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.phonenumbertxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.btnExportImage = new System.Windows.Forms.Button();
            this.printButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.profileimg = new System.Windows.Forms.PictureBox();
            this.ExportToExcelButton = new System.Windows.Forms.Button();
            this.clearbtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.profileimg)).BeginInit();
            this.SuspendLayout();
            // 
            // nametxt
            // 
            this.nametxt.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nametxt.Location = new System.Drawing.Point(378, 126);
            this.nametxt.Margin = new System.Windows.Forms.Padding(2);
            this.nametxt.Name = "nametxt";
            this.nametxt.Size = new System.Drawing.Size(285, 33);
            this.nametxt.TabIndex = 0;
            // 
            // transactionsGridView
            // 
            this.transactionsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.transactionsGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.transactionsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.transactionsGridView.Location = new System.Drawing.Point(23, 355);
            this.transactionsGridView.Margin = new System.Windows.Forms.Padding(2);
            this.transactionsGridView.Name = "transactionsGridView";
            this.transactionsGridView.RowHeadersWidth = 35;
            this.transactionsGridView.RowTemplate.Height = 24;
            this.transactionsGridView.Size = new System.Drawing.Size(888, 360);
            this.transactionsGridView.TabIndex = 2;
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.Black;
            this.nameLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.nameLabel.ForeColor = System.Drawing.Color.White;
            this.nameLabel.Location = new System.Drawing.Point(702, 121);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(157, 45);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "الاسم";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.Red;
            this.titleLabel.Location = new System.Drawing.Point(463, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(330, 88);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "عميل الجيم الميكس";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(800, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "الفئه";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Categorytxt
            // 
            this.Categorytxt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Categorytxt.Location = new System.Drawing.Point(682, 299);
            this.Categorytxt.Margin = new System.Windows.Forms.Padding(2);
            this.Categorytxt.Name = "Categorytxt";
            this.Categorytxt.Size = new System.Drawing.Size(113, 27);
            this.Categorytxt.TabIndex = 6;
            // 
            // membershipIDtxt
            // 
            this.membershipIDtxt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.membershipIDtxt.Location = new System.Drawing.Point(412, 299);
            this.membershipIDtxt.Margin = new System.Windows.Forms.Padding(2);
            this.membershipIDtxt.Name = "membershipIDtxt";
            this.membershipIDtxt.Size = new System.Drawing.Size(112, 27);
            this.membershipIDtxt.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(529, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "الاقدميه";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // phonenumbertxt
            // 
            this.phonenumbertxt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phonenumbertxt.Location = new System.Drawing.Point(42, 301);
            this.phonenumbertxt.Margin = new System.Windows.Forms.Padding(2);
            this.phonenumbertxt.Name = "phonenumbertxt";
            this.phonenumbertxt.Size = new System.Drawing.Size(184, 27);
            this.phonenumbertxt.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(231, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "تليفون";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.searchButton.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.Color.IndianRed;
            this.searchButton.Location = new System.Drawing.Point(702, 759);
            this.searchButton.Margin = new System.Windows.Forms.Padding(2);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(209, 58);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "بحث";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // btnExportImage
            // 
            this.btnExportImage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnExportImage.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportImage.ForeColor = System.Drawing.Color.IndianRed;
            this.btnExportImage.Location = new System.Drawing.Point(121, 261);
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(116, 35);
            this.btnExportImage.TabIndex = 12;
            this.btnExportImage.Text = "تحميل صوره";
            this.btnExportImage.UseVisualStyleBackColor = false;
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click_1);
            // 
            // printButton
            // 
            this.printButton.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.printButton.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printButton.ForeColor = System.Drawing.Color.IndianRed;
            this.printButton.Location = new System.Drawing.Point(23, 759);
            this.printButton.Margin = new System.Windows.Forms.Padding(2);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(205, 58);
            this.printButton.TabIndex = 13;
            this.printButton.Text = "طباعه";
            this.printButton.UseVisualStyleBackColor = false;
            this.printButton.Click += new System.EventHandler(this.printButton_Click_1);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Black;
            this.backButton.BackgroundImage = global::Private_Pool_Application.Properties.Resources._153_1531682_open_red_back_button_png;
            this.backButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.backButton.ForeColor = System.Drawing.Color.IndianRed;
            this.backButton.Location = new System.Drawing.Point(23, 12);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(70, 56);
            this.backButton.TabIndex = 22;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click_1);
            // 
            // profileimg
            // 
            this.profileimg.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.profileimg.Location = new System.Drawing.Point(47, 74);
            this.profileimg.Name = "profileimg";
            this.profileimg.Size = new System.Drawing.Size(264, 187);
            this.profileimg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.profileimg.TabIndex = 11;
            this.profileimg.TabStop = false;
            // 
            // ExportToExcelButton
            // 
            this.ExportToExcelButton.Location = new System.Drawing.Point(23, 720);
            this.ExportToExcelButton.Name = "ExportToExcelButton";
            this.ExportToExcelButton.Size = new System.Drawing.Size(70, 23);
            this.ExportToExcelButton.TabIndex = 25;
            this.ExportToExcelButton.Text = "استخراج ";
            this.ExportToExcelButton.UseVisualStyleBackColor = true;
            this.ExportToExcelButton.Click += new System.EventHandler(this.ExportToExcelButton_Click);
            // 
            // clearbtn
            // 
            this.clearbtn.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.clearbtn.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearbtn.ForeColor = System.Drawing.Color.IndianRed;
            this.clearbtn.Location = new System.Drawing.Point(270, 770);
            this.clearbtn.Margin = new System.Windows.Forms.Padding(2);
            this.clearbtn.Name = "clearbtn";
            this.clearbtn.Size = new System.Drawing.Size(117, 42);
            this.clearbtn.TabIndex = 26;
            this.clearbtn.Text = "مسح ";
            this.clearbtn.UseVisualStyleBackColor = false;
            this.clearbtn.Click += new System.EventHandler(this.clearbtn_Click);
            // 
            // CustomerReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(923, 889);
            this.Controls.Add(this.clearbtn);
            this.Controls.Add(this.ExportToExcelButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.btnExportImage);
            this.Controls.Add(this.profileimg);
            this.Controls.Add(this.phonenumbertxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.membershipIDtxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Categorytxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.transactionsGridView);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.nametxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CustomerReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Report";
            this.Load += new System.EventHandler(this.CustomerReport_Load_2);
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.profileimg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label nameLabel;
        private Label titleLabel;
        private Label label1;
        private TextBox Categorytxt;
        private TextBox membershipIDtxt;
        private Label label2;
        private TextBox phonenumbertxt;
        private Label label3;
        private PictureBox profileimg;
        private Button searchButton;
        private Button btnExportImage;
        private Button printButton;
        private Button backButton;
        private Button ExportToExcelButton;
        private Button clearbtn;
    }
}
