using System;
using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class AddEditCustomerWindow : Window
    {
        private readonly QlcuahangContext _context;
        private Customer _customer;

        public AddEditCustomerWindow(Customer customer = null)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _customer = customer;

            if (_customer != null)
            {
                // Editing existing customer
                NameTextBox.Text = _customer.Name;
                AddressTextBox.Text = _customer.Address;
                PhoneTextBox.Text = _customer.Phone;
                EmailTextBox.Text = _customer.Email;
                PointTextBox.Text = _customer.Point.ToString();
            }
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc.");
                return;
            }

            if (_customer == null)
            {
                // Adding new customer
                _customer = new Customer
                {
                    Name = NameTextBox.Text,
                    Address = AddressTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Email = EmailTextBox.Text,
                    Point = int.TryParse(PointTextBox.Text, out int point) ? point : 0
                };
                _context.Customers.Add(_customer);
            }
            else
            {
                // Updating existing customer
                _customer.Name = NameTextBox.Text;
                _customer.Address = AddressTextBox.Text;
                _customer.Phone = PhoneTextBox.Text;
                _customer.Email = EmailTextBox.Text;
                _customer.Point = int.TryParse(PointTextBox.Text, out int point) ? point : 0;
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}