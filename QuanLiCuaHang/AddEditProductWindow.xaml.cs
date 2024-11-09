using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class AddEditProductWindow : Window
    {
        private readonly QlcuahangContext _context;
        private Product _product;

        public AddEditProductWindow(Product product = null)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _product = product;

            if (_product != null)
            {
                // Editing existing product
                BarcodeTextBox.Text = _product.Barcode;
                TitleTextBox.Text = _product.Title;
                TypeTextBox.Text = _product.Type;
                ProductionSiteTextBox.Text = _product.ProductionSite;
                BarcodeTextBox.IsEnabled = false; // Không cho phép sửa mã sản phẩm
            }
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BarcodeTextBox.Text) || string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            if (_product == null)
            {
                // Adding new product
                _product = new Product
                {
                    Barcode = BarcodeTextBox.Text,
                    Title = TitleTextBox.Text,
                    Type = TypeTextBox.Text,
                    ProductionSite = ProductionSiteTextBox.Text
                };
                _context.Products.Add(_product);
            }
            else
            {
                // Updating existing product
                _product.Title = TitleTextBox.Text;
                _product.Type = TypeTextBox.Text;
                _product.ProductionSite = ProductionSiteTextBox.Text;
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}