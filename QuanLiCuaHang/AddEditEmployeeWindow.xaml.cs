using System;
using System.Windows;
using System.Windows.Controls;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class AddEditEmployeeWindow : Window
    {
        private readonly QlcuahangContext _context;
        private User _employee;

        public AddEditEmployeeWindow(User employee = null)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _employee = employee;

            if (_employee != null)
            {
                // Editing existing employee
                NameTextBox.Text = _employee.Name;
                RoleComboBox.SelectedItem = _employee.UserRole;
                AddressTextBox.Text = _employee.Address;
                PhoneTextBox.Text = _employee.Phone;
                EmailTextBox.Text = _employee.Email;
                UsernameTextBox.Text = _employee.UserName;
                SalaryTextBox.Text = _employee.Salary.ToString();

                UsernameTextBox.IsEnabled = false; // Không cho phép sửa tên đăng nhập
                PasswordBox.IsEnabled = false; // Không cho phép sửa mật khẩu trực tiếp
            }
        }

        private void SaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc.");
                return;
            }

            if (_employee == null)
            {
                // Adding new employee
                _employee = new User
                {
                    Name = NameTextBox.Text,
                    UserRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Address = AddressTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Email = EmailTextBox.Text,
                    UserName = UsernameTextBox.Text,
                    Password = PasswordBox.Password, // Trong thực tế, bạn nên mã hóa mật khẩu
                    Salary = int.TryParse(SalaryTextBox.Text, out int salary) ? salary : 0
                };
                _context.Users.Add(_employee);
            }
            else
            {
                // Updating existing employee
                _employee.Name = NameTextBox.Text;
                _employee.UserRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                _employee.Address = AddressTextBox.Text;
                _employee.Phone = PhoneTextBox.Text;
                _employee.Email = EmailTextBox.Text;
                _employee.Salary = int.TryParse(SalaryTextBox.Text, out int salary) ? salary : 0;
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}