using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class AddEditSupplierWindow : Window
    {
        private readonly QlcuahangContext _context;
        private Supplier _supplier;

        public AddEditSupplierWindow(Supplier supplier = null)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _supplier = supplier;

            if (_supplier != null)
            {
                // Editing existing supplier
                NameTextBox.Text = _supplier.Name;
                AddressTextBox.Text = _supplier.Address;
                PhoneTextBox.Text = _supplier.Phone;
                EmailTextBox.Text = _supplier.Email;
            }
        }

        private void SaveSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp.");
                return;
            }

            if (_supplier == null)
            {
                // Adding new supplier
                _supplier = new Supplier
                {
                    Name = NameTextBox.Text,
                    Address = AddressTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Email = EmailTextBox.Text
                };
                _context.Suppliers.Add(_supplier);
            }
            else
            {
                // Updating existing supplier
                _supplier.Name = NameTextBox.Text;
                _supplier.Address = AddressTextBox.Text;
                _supplier.Phone = PhoneTextBox.Text;
                _supplier.Email = EmailTextBox.Text;
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}