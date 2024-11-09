using System.Linq;
using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class AddProductToImportWindow : Window
    {
        private readonly QlcuahangContext _context;
        public ImportProductItem ImportProduct { get; private set; }

        public AddProductToImportWindow()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductComboBox.ItemsSource = _context.Products.ToList();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(InputPriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(OutputPriceTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            var product = ProductComboBox.SelectedItem as Product;
            ImportProduct = new ImportProductItem
            {
                Barcode = product.Barcode,
                Title = product.Title,
                Quantity = int.Parse(QuantityTextBox.Text),
                InputPrice = int.Parse(InputPriceTextBox.Text),
                OutputPrice = int.Parse(OutputPriceTextBox.Text)
            };

            DialogResult = true;
            Close();
        }
    }
}