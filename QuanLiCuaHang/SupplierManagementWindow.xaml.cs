using System.Linq;
using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class SupplierManagementWindow : Window
    {
        private readonly QlcuahangContext _context;

        public SupplierManagementWindow()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            SuppliersDataGrid.ItemsSource = _context.Suppliers.ToList();
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            var addEditSupplierWindow = new AddEditSupplierWindow();
            if (addEditSupplierWindow.ShowDialog() == true)
            {
                LoadSuppliers();
            }
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {
            var supplier = (sender as FrameworkElement).DataContext as Supplier;
            if (supplier != null)
            {
                var addEditSupplierWindow = new AddEditSupplierWindow(supplier);
                if (addEditSupplierWindow.ShowDialog() == true)
                {
                    LoadSuppliers();
                }
            }
        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            var supplier = (sender as FrameworkElement).DataContext as Supplier;
            if (supplier != null)
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp {supplier.Name}?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Suppliers.Remove(supplier);
                    _context.SaveChanges();
                    LoadSuppliers();
                }
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}