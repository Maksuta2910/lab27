namespace Lab27
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbDrives = new ComboBox();
            txtPath = new TextBox();
            btnUp = new Button();
            txtDirFilter = new TextBox();
            lstDirs = new ListBox();
            txtFileFilter = new TextBox();
            lstFiles = new ListBox();
            txtProperties = new TextBox();
            picPreview = new PictureBox();
            txtPreview = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            SuspendLayout();
            // 
            // cmbDrives
            // 
            cmbDrives.FormattingEnabled = true;
            cmbDrives.Location = new Point(11, 45);
            cmbDrives.Name = "cmbDrives";
            cmbDrives.Size = new Size(186, 28);
            cmbDrives.TabIndex = 0;
            cmbDrives.SelectedIndexChanged += cmbDrives_SelectedIndexChanged;
            // 
            // txtPath
            // 
            txtPath.Location = new Point(240, 46);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new Size(198, 27);
            txtPath.TabIndex = 1;
            txtPath.TextChanged += txtPath_TextChanged;
            // 
            // btnUp
            // 
            btnUp.Location = new Point(504, 12);
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(244, 27);
            btnUp.TabIndex = 2;
            btnUp.Text = "Вгору";
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += btnUp_Click;
            // 
            // txtDirFilter
            // 
            txtDirFilter.Location = new Point(12, 141);
            txtDirFilter.Name = "txtDirFilter";
            txtDirFilter.Size = new Size(186, 27);
            txtDirFilter.TabIndex = 3;
            txtDirFilter.Text = "*";
            txtDirFilter.TextChanged += txtDirFilter_TextChanged;
            // 
            // lstDirs
            // 
            lstDirs.FormattingEnabled = true;
            lstDirs.Location = new Point(11, 174);
            lstDirs.Name = "lstDirs";
            lstDirs.Size = new Size(186, 324);
            lstDirs.TabIndex = 4;
            lstDirs.SelectedIndexChanged += lstDirs_SelectedIndexChanged;
            lstDirs.DoubleClick += lstDirs_DoubleClick;
            // 
            // txtFileFilter
            // 
            txtFileFilter.Location = new Point(240, 141);
            txtFileFilter.Name = "txtFileFilter";
            txtFileFilter.Size = new Size(198, 27);
            txtFileFilter.TabIndex = 5;
            txtFileFilter.Text = "*.*";
            txtFileFilter.TextChanged += txtFileFilter_TextChanged;
            // 
            // lstFiles
            // 
            lstFiles.FormattingEnabled = true;
            lstFiles.Location = new Point(240, 174);
            lstFiles.Name = "lstFiles";
            lstFiles.Size = new Size(198, 324);
            lstFiles.TabIndex = 6;
            lstFiles.SelectedIndexChanged += lstFiles_SelectedIndexChanged;
            // 
            // txtProperties
            // 
            txtProperties.Location = new Point(504, 72);
            txtProperties.Multiline = true;
            txtProperties.Name = "txtProperties";
            txtProperties.ScrollBars = ScrollBars.Vertical;
            txtProperties.Size = new Size(244, 144);
            txtProperties.TabIndex = 7;
            // 
            // picPreview
            // 
            picPreview.Location = new Point(504, 242);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(244, 169);
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 8;
            picPreview.TabStop = false;
            // 
            // txtPreview
            // 
            txtPreview.Location = new Point(504, 417);
            txtPreview.Multiline = true;
            txtPreview.Name = "txtPreview";
            txtPreview.ScrollBars = ScrollBars.Vertical;
            txtPreview.Size = new Size(244, 79);
            txtPreview.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(102, 20);
            label1.TabIndex = 10;
            label1.Text = "Оберіть диск:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(240, 15);
            label2.Name = "label2";
            label2.Size = new Size(121, 20);
            label2.TabIndex = 11;
            label2.Text = "Поточний шлях:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 118);
            label3.Name = "label3";
            label3.Size = new Size(134, 20);
            label3.TabIndex = 12;
            label3.Text = "Каталоги (фільтр):";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(240, 118);
            label4.Name = "label4";
            label4.Size = new Size(116, 20);
            label4.TabIndex = 13;
            label4.Text = "Файли (фільтр):";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(504, 49);
            label5.Name = "label5";
            label5.Size = new Size(163, 20);
            label5.TabIndex = 14;
            label5.Text = "Властивості / Безпека:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(504, 219);
            label6.Name = "label6";
            label6.Size = new Size(164, 20);
            label6.TabIndex = 15;
            label6.Text = "Попередній перегляд:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 508);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtPreview);
            Controls.Add(picPreview);
            Controls.Add(txtProperties);
            Controls.Add(lstFiles);
            Controls.Add(txtFileFilter);
            Controls.Add(lstDirs);
            Controls.Add(txtDirFilter);
            Controls.Add(btnUp);
            Controls.Add(txtPath);
            Controls.Add(cmbDrives);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbDrives;
        private TextBox txtPath;
        private Button btnUp;
        private TextBox txtDirFilter;
        private ListBox lstDirs;
        private TextBox txtFileFilter;
        private ListBox lstFiles;
        private TextBox txtProperties;
        private PictureBox picPreview;
        private TextBox txtPreview;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}
