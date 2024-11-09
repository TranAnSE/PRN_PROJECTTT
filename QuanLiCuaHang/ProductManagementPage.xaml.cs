using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class ProductManagementPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public ProductManagementPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsDataGrid.ItemsSource = _context.Products.ToList();
        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddEditProductWindow();
            if (addProductWindow.ShowDialog() == true)
            {
                LoadProducts();
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button).DataContext as Product;
            if (product != null)
            {
                var editProductWindow = new AddEditProductWindow(product);
                if (editProductWindow.ShowDialog() == true)
                {
                    LoadProducts();
                }
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button).DataContext as Product;
            if (product != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                    LoadProducts();
                }
            }
        }
    }
}