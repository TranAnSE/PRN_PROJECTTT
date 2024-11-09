using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class ImportProductPage : UserControl
    {
        private readonly QlcuahangContext _context;
        private ObservableCollection<ImportProductItem> _importProducts;

        public ImportProductPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _importProducts = new ObservableCollection<ImportProductItem>();
            ProductsDataGrid.ItemsSource = _importProducts;
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            SupplierComboBox.ItemsSource = _context.Suppliers.ToList();
        }

        private void ManageSuppliers_Click(object sender, RoutedEventArgs e)
        {
            var supplierManagementWindow = new SupplierManagementWindow();
            if (supplierManagementWindow.ShowDialog() == true)
            {
                LoadSuppliers();
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductToImportWindow();
            if (addProductWindow.ShowDialog() == true)
            {
                _importProducts.Add(addProductWindow.ImportProduct);
            }
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button).DataContext as ImportProductItem;
            if (product != null)
            {
                _importProducts.Remove(product);
            }
        }

        private void SaveImport_Click(object sender, RoutedEventArgs e)
        {
            if (SupplierComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp.");
                return;
            }

            if (_importProducts.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào phiếu nhập.");
                return;
            }

            var supplier = SupplierComboBox.SelectedItem as Supplier;
            var inputInfo = new InputInfo
            {
                InputDate = DateOnly.FromDateTime(DateTime.Now),
                SupplierId = supplier.Id,
                UserId = 1 // Thay thế bằng ID của người dùng hiện tại
            };

            _context.InputInfos.Add(inputInfo);
            _context.SaveChanges();

            foreach (var item in _importProducts)
            {
                var consignment = new Consignment
                {
                    InputInfoId = inputInfo.Id,
                    ProductId = item.Barcode,
                    Stock = item.Quantity,
                    ManufacturingDate = DateOnly.FromDateTime(DateTime.Now), // Thay thế bằng ngày sản xuất thực tế
                    ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1)), // Thay thế bằng hạn sử dụng thực tế
                    InputPrice = item.InputPrice,
                    OutputPrice = item.OutputPrice,
                    InStock = item.Quantity
                };

                _context.Consignments.Add(consignment);
            }

            _context.SaveChanges();
            MessageBox.Show("Đã lưu phiếu nhập hàng thành công.");
            _importProducts.Clear();
        }
    }

    public class ImportProductItem
    {
        public string Barcode { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public int InputPrice { get; set; }
        public int OutputPrice { get; set; }
    }
}