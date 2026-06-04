using System;
using System.IO;
using System.Drawing;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;

namespace Lab27
{
    public partial class Form1 : Form
    {
        private string currentPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        // Завантаження форми: отримуємо список дисків
        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                cmbDrives.Items.Add(drive.Name);
            }
            if (cmbDrives.Items.Count > 0)
                cmbDrives.SelectedIndex = 0; // Автоматично вибираємо перший диск
        }

        // При виборі диска
        private void cmbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            string driveName = cmbDrives.SelectedItem?.ToString() ?? "";
            DriveInfo drive = new DriveInfo(driveName);

            // Виведення властивостей диска
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

        // Метод завантаження вмісту каталогу
        private void LoadDirectory(string path)
        {
            try
            {
                currentPath = path;
                txtPath.Text = currentPath;
                lstDirs.Items.Clear();
                lstFiles.Items.Clear();

                DirectoryInfo dir = new DirectoryInfo(path);

                // Застосування фільтрів
                string dirFilter = string.IsNullOrWhiteSpace(txtDirFilter.Text) ? "*" : txtDirFilter.Text;
                string fileFilter = string.IsNullOrWhiteSpace(txtFileFilter.Text) ? "*.*" : txtFileFilter.Text;

                // Завантаження каталогів
                foreach (DirectoryInfo d in dir.GetDirectories(dirFilter))
                {
                    lstDirs.Items.Add(d.Name);
                }

                // Завантаження файлів
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

        // Перехід у вибраний каталог (подвійний клік)
        private void lstDirs_DoubleClick(object sender, EventArgs e)
        {
            if (lstDirs.SelectedItem != null)
            {
                string newPath = Path.Combine(currentPath, lstDirs.SelectedItem?.ToString() ?? "");
                LoadDirectory(newPath);
            }
        }

        // Кнопка "Вгору"
        private void btnUp_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(currentPath);
            if (dirInfo.Parent != null)
            {
                LoadDirectory(dirInfo.Parent.FullName);
            }
        }

        // Перегляд властивостей каталогу (один клік)
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

            // Атрибути безпеки (Власник)
            try
            {
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                txtProperties.Text += $"Власник: {dSecurity.GetOwner(typeof(NTAccount))?.Value}\r\n";
            }
            catch { txtProperties.Text += "Власник: Немає доступу\r\n"; }
        }

        // Перегляд властивостей та прев'ю файлу
        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;

            string fullPath = Path.Combine(currentPath, lstFiles.SelectedItem?.ToString() ?? "");
            FileInfo fInfo = new FileInfo(fullPath);

            // Властивості та атрибути безпеки
            txtProperties.Text = $"=== ВЛАСТИВОСТІ ФАЙЛУ ===\r\n";
            txtProperties.Text += $"Ім'я: {fInfo.Name}\r\n";
            txtProperties.Text += $"Розмір: {fInfo.Length} байт\r\n";
            txtProperties.Text += $"Розширення: {fInfo.Extension}\r\n";
            txtProperties.Text += $"Створено: {fInfo.CreationTime}\r\n";
            txtProperties.Text += $"Атрибути: {fInfo.Attributes}\r\n";

            try
            {
                FileSecurity fSecurity = fInfo.GetAccessControl();
                txtProperties.Text += $"Власник: {fSecurity.GetOwner(typeof(NTAccount))?.Value}\r\n";
            }
            catch { txtProperties.Text += "Власник: Немає доступу\r\n"; }

            // Перегляд вмісту (Графіка або Текст)
            PreviewFile(fInfo);
        }

        private void PreviewFile(FileInfo fInfo)
        {
            txtPreview.Text = "";
            picPreview.Image = null;
            string ext = fInfo.Extension.ToLower();

            try
            {
                // Графічні файли
                if (ext == ".jpg" || ext == ".png" || ext == ".bmp" || ext == ".gif")
                {
                    using (FileStream fs = new FileStream(fInfo.FullName, FileMode.Open, FileAccess.Read))
                    {
                        picPreview.Image = Image.FromStream(fs);
                    }
                }
                // Текстові файли
                else if (ext == ".txt" || ext == ".log" || ext == ".cs" || ext == ".xml")
                {
                    // Читаємо тільки перші 4 КБ, щоб не зависла програма від великих файлів
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

        // Обробка зміни фільтрів (оновлення списків)
        private void txtDirFilter_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrEmpty(currentPath)) LoadDirectory(currentPath); }
        private void txtFileFilter_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrEmpty(currentPath)) LoadDirectory(currentPath); }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}