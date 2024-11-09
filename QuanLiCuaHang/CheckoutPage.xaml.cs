using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class CheckoutPage : UserControl
    {
        private readonly QlcuahangContext _context;
        private ObservableCollection<CartItem> _cartItems;
        private List<dynamic> _allProducts;
        private Voucher _appliedVoucher;

        public CheckoutPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _cartItems = new ObservableCollection<CartItem>();
            CartListBox.ItemsSource = _cartItems;
            LoadProducts();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            CustomerComboBox.ItemsSource = _context.Customers.ToList();
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AddEditCustomerWindow();
            if (addCustomerWindow.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng.");
                return;
            }

            var customer = CustomerComboBox.SelectedItem as Customer;
            var total = _cartItems.Sum(item => item.Price * item.Quantity);
            var discount = _appliedVoucher?.Block?.ParValue ?? 0;

            var bill = new Bill
            {
                BillDate = DateTime.Now,
                CustomerId = customer.Id,
                UserId = 1, // Replace with actual logged-in user ID
                Price = total - discount,
                Discount = discount
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            foreach (var item in _cartItems)
            {
                var billDetail = new BillDetail
                {
                    BillId = bill.Id,
                    ProductId = item.Product.Barcode,
                    Quantity = item.Quantity,
                    TotalPrice = item.Price * item.Quantity
                };

                _context.BillDetails.Add(billDetail);

                var consignment = _context.Consignments
                    .Where(c => c.ProductId == item.Product.Barcode)
                    .OrderByDescending(c => c.InputInfoId)
                    .FirstOrDefault();

                if (consignment != null)
                {
                    consignment.InStock -= item.Quantity;
                }
            }

            if (_appliedVoucher != null)
            {
                _appliedVoucher.Status = 1; // Đánh dấu voucher đã sử dụng
                _context.Vouchers.Update(_appliedVoucher);
            }

            _context.SaveChanges();

            MessageBox.Show("Thanh toán thành công!");
            _cartItems.Clear();
            _appliedVoucher = null;
            VoucherTextBox.Text = string.Empty;
            UpdateTotal();
        }

        private void LoadProducts()
        {
            _allProducts = _context.Products
                .Include(p => p.Consignments)
                .Select(p => new
                {
                    p.Barcode,
                    p.Title,
                    Price = p.Consignments.OrderByDescending(c => c.InputInfoId)
                                          .Select(c => c.OutputPrice)
                                          .FirstOrDefault() ?? 0,
                    InStock = p.Consignments.Sum(c => c.InStock)
                })
                .ToList<dynamic>();

            ProductsDataGrid.ItemsSource = _allProducts;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button)?.DataContext as dynamic;
            if (product != null)
            {
                var cartItem = _cartItems.FirstOrDefault(item => item.Product.Barcode == product.Barcode);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    _cartItems.Add(new CartItem
                    {
                        Product = new Product { Barcode = product.Barcode, Title = product.Title },
                        Quantity = 1,
                        Price = product.Price ?? 0
                    });
                }
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            var total = _cartItems.Sum(item => item.Price * item.Quantity);
            var discount = _appliedVoucher?.Block?.ParValue ?? 0;
            TotalTextBlock.Text = $"{total - discount:N0} VND";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.ToLower();
            var filteredProducts = _allProducts.Where(p =>
                p.Barcode.ToString().ToLower().Contains(searchTerm) ||
                p.Title.ToString().ToLower().Contains(searchTerm)
            ).ToList();

            ProductsDataGrid.ItemsSource = filteredProducts;
        }

        private void ApplyVoucher_Click(object sender, RoutedEventArgs e)
        {
            var voucherCode = VoucherTextBox.Text.Trim();
            var voucher = _context.Vouchers
                .Include(v => v.Block)
                .FirstOrDefault(v => v.Code == voucherCode && v.Status == 0);

            if (voucher != null && voucher.Block != null)
            {
                _appliedVoucher = voucher;
                MessageBox.Show($"Áp dụng voucher thành công. Giảm giá: {voucher.Block.ParValue:N0} VND");
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("Voucher không hợp lệ hoặc đã được sử dụng.");
            }
        }

        public class CartItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public int Price { get; set; }
        }
    }
}