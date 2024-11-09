using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class UpdateInventoryWindow : Window
    {
        private readonly QlcuahangContext _context;
        private readonly string _productId;

        public UpdateInventoryWindow(string productId)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _productId = productId;
            LoadProductInfo();
        }

        private void LoadProductInfo()
        {
            var product = _context.Products
                .Include(p => p.Consignments)
                .FirstOrDefault(p => p.Barcode == _productId);

            if (product != null)
            {
                ProductIdTextBox.Text = product.Barcode;
                ProductNameTextBox.Text = product.Title;
                InStockTextBox.Text = product.Consignments.Sum(c => c.InStock).ToString();

                var latestConsignment = product.Consignments.OrderByDescending(c => c.InputInfoId).FirstOrDefault();
                if (latestConsignment != null)
                {
                    InputPriceTextBox.Text = latestConsignment.InputPrice.ToString();
                    OutputPriceTextBox.Text = latestConsignment.OutputPrice.ToString();
                }
            }
        }

        private void UpdateInventory_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(InStockTextBox.Text, out int inStock) ||
                !int.TryParse(InputPriceTextBox.Text, out int inputPrice) ||
                !int.TryParse(OutputPriceTextBox.Text, out int outputPrice))
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ cho số lượng và giá.");
                return;
            }

            var product = _context.Products
                .Include(p => p.Consignments)
                .FirstOrDefault(p => p.Barcode == _productId);

            if (product != null)
            {
                var latestConsignment = product.Consignments.OrderByDescending(c => c.InputInfoId).FirstOrDefault();
                if (latestConsignment != null)
                {
                    latestConsignment.InStock = inStock;
                    latestConsignment.InputPrice = inputPrice;
                    latestConsignment.OutputPrice = outputPrice;
                    _context.SaveChanges();
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin lô hàng cho sản phẩm này.");
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm.");
            }
        }
    }
}