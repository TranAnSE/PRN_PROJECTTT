using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class SettingsPage : UserControl
    {
        private readonly QlcuahangContext _context;
        private User _currentUser;

        public SettingsPage(User currentUser)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _currentUser = currentUser;
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            NameTextBox.Text = _currentUser.Name;
            EmailTextBox.Text = _currentUser.Email;
            PhoneTextBox.Text = _currentUser.Phone;
        }

        private void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            _currentUser.Name = NameTextBox.Text;
            _currentUser.Email = EmailTextBox.Text;
            _currentUser.Phone = PhoneTextBox.Text;

            _context.Entry(_currentUser).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("Thông tin đã được cập nhật!");
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OldPasswordBox.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cũ.");
                return;
            }

            if (OldPasswordBox.Password != _currentUser.Password)
            {
                MessageBox.Show("Mật khẩu cũ không đúng!");
                return;
            }

            if (string.IsNullOrEmpty(NewPasswordBox.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.");
                return;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Mật khẩu mới không khớp!");
                return;
            }

            _currentUser.Password = NewPasswordBox.Password;
            _context.Entry(_currentUser).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("Mật khẩu đã được thay đổi!");

            OldPasswordBox.Clear();
            NewPasswordBox.Clear();
            ConfirmPasswordBox.Clear();
        }
        private void BackupDatabase_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Backup files (*.bak)|*.bak",
                DefaultExt = "bak",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                DatabaseManager.BackupDatabase(saveFileDialog.FileName);
            }
        }

        private void RestoreDatabase_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Backup files (*.bak)|*.bak",
                DefaultExt = "bak"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                DatabaseManager.RestoreDatabase(openFileDialog.FileName);
            }
        }
    }
}