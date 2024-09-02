namespace Private_Pool_Application
{
    partial class UserUpdate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserUpdate));
            this.loadtbtn = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.backButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.updatebtn = new System.Windows.Forms.Button();
            this.usersDataGridView = new System.Windows.Forms.DataGridView();
            this.updatetransbtn = new System.Windows.Forms.Button();
            this.nametxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // loadtbtn
            // 
            this.loadtbtn.BackColor = System.Drawing.Color.Black;
            this.loadtbtn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadtbtn.ForeColor = System.Drawing.Color.IndianRed;
            this.loadtbtn.Location = new System.Drawing.Point(662, 489);
            this.loadtbtn.Margin = new System.Windows.Forms.Padding(2);
            this.loadtbtn.Name = "loadtbtn";
            this.loadtbtn.Size = new System.Drawing.Size(180, 68);
            this.loadtbtn.TabIndex = 26;
            this.loadtbtn.Text = "بحث";
            this.loadtbtn.UseVisualStyleBackColor = false;
            this.loadtbtn.Click += new System.EventHandler(this.loadbtn_Click);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Black;
            this.backButton.BackgroundImage = global::Private_Pool_Application.Properties.Resources._153_1531682_open_red_back_button_png;
            this.backButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.backButton.ForeColor = System.Drawing.Color.IndianRed;
            this.backButton.Location = new System.Drawing.Point(5, 10);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(70, 56);
            this.backButton.TabIndex = 30;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.Red;
            this.titleLabel.Location = new System.Drawing.Point(281, 39);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(287, 44);
            this.titleLabel.TabIndex = 28;
            this.titleLabel.Text = "تعديل بيانات الشخصيه";
            // 
            // updatebtn
            // 
            this.updatebtn.BackColor = System.Drawing.Color.Black;
            this.updatebtn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatebtn.ForeColor = System.Drawing.Color.IndianRed;
            this.updatebtn.Location = new System.Drawing.Point(21, 489);
            this.updatebtn.Margin = new System.Windows.Forms.Padding(2);
            this.updatebtn.Name = "updatebtn";
            this.updatebtn.Size = new System.Drawing.Size(180, 68);
            this.updatebtn.TabIndex = 32;
            this.updatebtn.Text = "تعديل";
            this.updatebtn.UseVisualStyleBackColor = false;
            this.updatebtn.Click += new System.EventHandler(this.updatebtn_Click_1);
            // 
            // usersDataGridView
            // 
            this.usersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usersDataGridView.Location = new System.Drawing.Point(21, 102);
            this.usersDataGridView.Name = "usersDataGridView";
            this.usersDataGridView.Size = new System.Drawing.Size(821, 353);
            this.usersDataGridView.TabIndex = 33;
           // this.usersDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.usersDataGridView_CellContentClick);
            // 
            // updatetransbtn
            // 
            this.updatetransbtn.BackColor = System.Drawing.Color.Black;
            this.updatetransbtn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatetransbtn.ForeColor = System.Drawing.Color.IndianRed;
            this.updatetransbtn.Location = new System.Drawing.Point(704, 16);
            this.updatetransbtn.Margin = new System.Windows.Forms.Padding(2);
            this.updatetransbtn.Name = "updatetransbtn";
            this.updatetransbtn.Size = new System.Drawing.Size(146, 41);
            this.updatetransbtn.TabIndex = 34;
            this.updatetransbtn.Text = "تعديل حسابات";
            this.updatetransbtn.UseVisualStyleBackColor = false;
            this.updatetransbtn.Click += new System.EventHandler(this.updatetransbtn_Click);
            // 
            // nametxt
            // 
            this.nametxt.Location = new System.Drawing.Point(368, 508);
            this.nametxt.Name = "nametxt";
            this.nametxt.Size = new System.Drawing.Size(274, 20);
            this.nametxt.TabIndex = 35;
            this.nametxt.TextChanged += new System.EventHandler(this.nametxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(503, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 36;
            this.label1.Text = "بحث عن طريق الاسم";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // UserUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(861, 598);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nametxt);
            this.Controls.Add(this.updatetransbtn);
            this.Controls.Add(this.usersDataGridView);
            this.Controls.Add(this.updatebtn);
            this.Controls.Add(this.loadtbtn);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign in";
            this.Load += new System.EventHandler(this.Loginform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button loadtbtn;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button updatebtn;
        private System.Windows.Forms.DataGridView usersDataGridView;
        private System.Windows.Forms.Button updatetransbtn;
        private System.Windows.Forms.TextBox nametxt;
        private System.Windows.Forms.Label label1;
    }
}