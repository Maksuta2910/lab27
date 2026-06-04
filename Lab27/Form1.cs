using System;
using System.IO;
using System.Drawing;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO.Compression;

namespace Lab27
{
    public partial class Form1 : Form
    {
        private string currentPath = "";
        private bool isUpdatingUI = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                cmbDrives.Items.Add(drive.Name);
            }
            if (cmbDrives.Items.Count > 0)
                cmbDrives.SelectedIndex = 0;
        }

        private void cmbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            string driveName = cmbDrives.SelectedItem?.ToString() ?? "";
            DriveInfo drive = new DriveInfo(driveName);

            txtProperties.Text = $"=== ВЛАСТИВОСТІ ДИСКУ ===\r\n";
            txtProperties.Text += $"Назва: {drive.Name}\r\n";
            txtProperties.Text += $"Тип: {drive.DriveType}\r\n";

            if (drive.IsReady)
            {
                txtProperties.Text += $"Файлова система: {drive.DriveFormat}\r\n";
                txtProperties.Text += $"Загальний розмір: {drive.TotalSize / (1024 * 1024)} МБ\r\n";
                txtProperties.Text += $"Вільний простір: {drive.TotalFreeSpace / (1024 * 1024)} МБ\r\n";
                txtProperties.Text += $"Мітка: {drive.VolumeLabel}\r\n";

                LoadDirectory(drive.RootDirectory.FullName);
            }
            else
            {
                txtProperties.Text += "Диск не готовий (можливо, відсутній носій).\r\n";
                lstDirs.Items.Clear();
                lstFiles.Items.Clear();
            }
        }

        private void LoadDirectory(string path)
        {
            try
            {
                currentPath = path;
                txtPath.Text = currentPath;
                lstDirs.Items.Clear();
                lstFiles.Items.Clear();

                DirectoryInfo dir = new DirectoryInfo(path);

                string dirFilter = string.IsNullOrWhiteSpace(txtDirFilter.Text) ? "*" : txtDirFilter.Text;
                string fileFilter = string.IsNullOrWhiteSpace(txtFileFilter.Text) ? "*.*" : txtFileFilter.Text;

                foreach (DirectoryInfo d in dir.GetDirectories(dirFilter))
                {
                    lstDirs.Items.Add(d.Name);
                }

                foreach (FileInfo f in dir.GetFiles(fileFilter))
                {
                    lstFiles.Items.Add(f.Name);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Відмовлено в доступі до цієї папки (Брак прав).", "Помилка безпеки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstDirs_DoubleClick(object sender, EventArgs e)
        {
            if (lstDirs.SelectedItem != null)
            {
                string newPath = Path.Combine(currentPath, lstDirs.SelectedItem?.ToString() ?? "");
                LoadDirectory(newPath);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(currentPath);
            if (dirInfo.Parent != null)
            {
                LoadDirectory(dirInfo.Parent.FullName);
            }
        }

        private void lstDirs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDirs.SelectedItem == null) return;

            string fullPath = Path.Combine(currentPath, lstDirs.SelectedItem?.ToString() ?? "");
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);

            txtProperties.Text = $"=== ВЛАСТИВОСТІ КАТАЛОГУ ===\r\n";
            txtProperties.Text += $"Ім'я: {dInfo.Name}\r\n";
            txtProperties.Text += $"Повний шлях: {dInfo.FullName}\r\n";
            txtProperties.Text += $"Створено: {dInfo.CreationTime}\r\n";
            txtProperties.Text += $"Остання зміна: {dInfo.LastWriteTime}\r\n";
            txtProperties.Text += $"Атрибути: {dInfo.Attributes}\r\n";

            try
            {
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                txtProperties.Text += $"Власник: {dSecurity.GetOwner(typeof(NTAccount))?.Value}\r\n";
            }
            catch { txtProperties.Text += "Власник: Немає доступу\r\n"; }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;

            string fullPath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");
            FileInfo fInfo = new FileInfo(fullPath);

            txtProperties.Text = $"=== ВЛАСТИВОСТІ ФАЙЛУ ===\r\n";
            txtProperties.Text += $"Ім'я: {fInfo.Name}\r\n";
            txtProperties.Text += $"Розмір: {fInfo.Length} байт\r\n";
            txtProperties.Text += $"Розширення: {fInfo.Extension}\r\n";
            txtProperties.Text += $"Створено: {fInfo.CreationTime}\r\n";
            txtProperties.Text += $"Атрибути: {fInfo.Attributes}\r\n";

            isUpdatingUI = true;
            if (chkReadOnly != null) chkReadOnly.Checked = (fInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            if (chkHidden != null) chkHidden.Checked = (fInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            isUpdatingUI = false;

            try
            {
                FileSecurity fSecurity = fInfo.GetAccessControl();
                txtProperties.Text += $"Власник: {fSecurity.GetOwner(typeof(NTAccount))?.Value}\r\n";
            }
            catch { txtProperties.Text += "Власник: Немає доступу\r\n"; }

            PreviewFile(fInfo);
        }

        private void PreviewFile(FileInfo fInfo)
        {
            txtPreview.Text = "";
            picPreview.Image = null;
            string ext = fInfo.Extension.ToLower();

            try
            {
                if (ext == ".jpg" || ext == ".png" || ext == ".bmp" || ext == ".gif")
                {
                    using (FileStream fs = new FileStream(fInfo.FullName, FileMode.Open, FileAccess.Read))
                    {
                        picPreview.Image = Image.FromStream(fs);
                    }
                }
                else if (ext == ".txt" || ext == ".log" || ext == ".cs" || ext == ".xml")
                {
                    using (StreamReader sr = new StreamReader(fInfo.FullName))
                    {
                        char[] buffer = new char[4096];
                        int bytesRead = sr.Read(buffer, 0, buffer.Length);
                        txtPreview.Text = new string(buffer, 0, bytesRead);
                    }
                }
                else
                {
                    txtPreview.Text = "Попередній перегляд для цього типу файлу не підтримується.";
                }
            }
            catch (Exception ex)
            {
                txtPreview.Text = "Помилка читання файлу: " + ex.Message;
            }
        }

        // ==========================================
        // НОВІ ФУНКЦІЇ ДЛЯ ЛАБОРАТОРНОЇ РОБОТИ №28
        // ==========================================

        private void btnCreateDir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInputName.Text) || string.IsNullOrEmpty(currentPath)) return;
            string newDirPath = Path.Combine(currentPath, txtInputName.Text);
            try
            {
                if (!Directory.Exists(newDirPath))
                {
                    Directory.CreateDirectory(newDirPath);
                    LoadDirectory(currentPath);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInputName.Text) || string.IsNullOrEmpty(currentPath)) return;
            string newFilePath = Path.Combine(currentPath, txtInputName.Text);
            try
            {
                if (!File.Exists(newFilePath))
                {
                    using (File.Create(newFilePath)) { }
                    LoadDirectory(currentPath);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstFiles.SelectedItem != null)
                {
                    string filePath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");
                    File.Delete(filePath);
                }
                else if (lstDirs.SelectedItem != null)
                {
                    string dirPath = Path.Combine(currentPath, lstDirs.SelectedItem?.ToString() ?? "");
                    Directory.Delete(dirPath, true);
                }
                LoadDirectory(currentPath);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string sourcePath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string destPath = Path.Combine(fbd.SelectedPath, lstFiles.SelectedItem?.ToString() ?? "");
                    try
                    {
                        File.Copy(sourcePath, destPath, true);
                        MessageBox.Show("Файл скопійовано успішно!");
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string sourcePath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string destPath = Path.Combine(fbd.SelectedPath, lstFiles.SelectedItem?.ToString() ?? "");
                    try
                    {
                        File.Move(sourcePath, destPath);
                        LoadDirectory(currentPath);
                        MessageBox.Show("Файл переміщено успішно!");
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }

        private void btnSaveText_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string filePath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");
            try
            {
                File.WriteAllText(filePath, txtPreview.Text);
                MessageBox.Show("Зміни збережено.");
                LoadDirectory(currentPath);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnZip_Click(object sender, EventArgs e)
        {
            if (lstDirs.SelectedItem == null) return;
            string sourceDir = Path.Combine(currentPath, lstDirs.SelectedItem?.ToString() ?? "");
            string zipPath = sourceDir + ".zip";

            try
            {
                ZipFile.CreateFromDirectory(sourceDir, zipPath);
                LoadDirectory(currentPath);
                MessageBox.Show("Папку успішно заархівовано.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnUnzip_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string zipPath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");
            if (!zipPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)) return;
        }
        private void chkAttributes_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdatingUI || lstFiles.SelectedItem == null) return;

            string filePath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");

            try
            {
                FileAttributes attributes = File.GetAttributes(filePath);

                if (chkReadOnly.Checked) attributes |= FileAttributes.ReadOnly;
                else attributes &= ~FileAttributes.ReadOnly;

                if (chkHidden.Checked) attributes |= FileAttributes.Hidden;
                else attributes &= ~FileAttributes.Hidden;

                File.SetAttributes(filePath, attributes);

                int index = lstFiles.SelectedIndex;
                LoadDirectory(currentPath);
                if (lstFiles.Items.Count > index) lstFiles.SelectedIndex = index;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void txtDirFilter_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrEmpty(currentPath)) LoadDirectory(currentPath); }
        private void txtFileFilter_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrEmpty(currentPath)) LoadDirectory(currentPath); }
        private void txtPath_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
    }
}
