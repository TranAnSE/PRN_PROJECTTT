using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class SalaryPaymentPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public SalaryPaymentPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            EmployeesDataGrid.ItemsSource = _context.Users.ToList();
        }

        private void PaySalary_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as User;
            if (employee != null)
            {
                var salaryBill = new SalaryBill
                {
                    SalaryBillDate = DateOnly.FromDateTime(DateTime.Now),
                    UserId = employee.Id,
                    TotalMoney = employee.Salary
                };

                _context.SalaryBills.Add(salaryBill);
                employee.SalaryDate = DateOnly.FromDateTime(DateTime.Now);
                _context.SaveChanges();

                MessageBox.Show($"Đã trả lương cho nhân viên {employee.Name}");
                LoadEmployees();
            }
        }
    }
}