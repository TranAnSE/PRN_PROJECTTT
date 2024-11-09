using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class EmployeeManagementPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public EmployeeManagementPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            EmployeesDataGrid.ItemsSource = _context.Users.ToList();
        }

        private void AddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEditEmployeeWindow();
            if (addEmployeeWindow.ShowDialog() == true)
            {
                LoadEmployees();
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as User;
            if (employee != null)
            {
                var editEmployeeWindow = new AddEditEmployeeWindow(employee);
                if (editEmployeeWindow.ShowDialog() == true)
                {
                    LoadEmployees();
                }
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as User;
            if (employee != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Users.Remove(employee);
                    _context.SaveChanges();
                    LoadEmployees();
                }
            }
        }
    }
}