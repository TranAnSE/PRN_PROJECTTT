using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class CustomerManagementPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public CustomerManagementPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            CustomersDataGrid.ItemsSource = _context.Customers.ToList();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AddEditCustomerWindow();
            if (addCustomerWindow.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = (sender as Button).DataContext as Customer;
            if (customer != null)
            {
                var editCustomerWindow = new AddEditCustomerWindow(customer);
                if (editCustomerWindow.ShowDialog() == true)
                {
                    LoadCustomers();
                }
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = (sender as Button).DataContext as Customer;
            if (customer != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Customers.Remove(customer);
                    _context.SaveChanges();
                    LoadCustomers();
                }
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = SearchTextBox.Text.ToLower();
            var filteredCustomers = _context.Customers
                .Where(c => c.Name.ToLower().Contains(searchTerm) ||
                            c.Phone.Contains(searchTerm) ||
                            c.Email.ToLower().Contains(searchTerm))
                .ToList();

            CustomersDataGrid.ItemsSource = filteredCustomers;
        }
    }
}