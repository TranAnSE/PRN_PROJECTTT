using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class SupplierManagementPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public SupplierManagementPage()
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
            var addSupplierWindow = new AddEditSupplierWindow();
            if (addSupplierWindow.ShowDialog() == true)
            {
                LoadSuppliers();
            }
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {
            var supplier = (sender as Button).DataContext as Supplier;
            if (supplier != null)
            {
                var editSupplierWindow = new AddEditSupplierWindow(supplier);
                if (editSupplierWindow.ShowDialog() == true)
                {
                    LoadSuppliers();
                }
            }
        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            var supplier = (sender as Button).DataContext as Supplier;
            if (supplier != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Suppliers.Remove(supplier);
                    _context.SaveChanges();
                    LoadSuppliers();
                }
            }
        }
    }
}