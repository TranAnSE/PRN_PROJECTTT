using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class SalesReportPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public SalesReportPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            StartDatePicker.SelectedDate = DateTime.Today.AddDays(-30);
            EndDatePicker.SelectedDate = DateTime.Today;
            LoadReport();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            var startDate = StartDatePicker.SelectedDate ?? DateTime.Today.AddDays(-30);
            var endDate = EndDatePicker.SelectedDate ?? DateTime.Today;
            endDate = endDate.AddDays(1).AddTicks(-1); // Set to end of day

            var bills = _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.BillDetails)
                .Where(b => b.BillDate >= startDate && b.BillDate <= endDate)
                .ToList();

            // Tổng quan
            var totalSales = bills.Sum(b => b.Price);
            var totalOrders = bills.Count;
            var averageOrderValue = totalOrders > 0 ? totalSales / totalOrders : 0;

            TotalSalesTextBlock.Text = $"Tổng doanh số: {totalSales:N0} VND";
            TotalOrdersTextBlock.Text = $"Tổng số đơn hàng: {totalOrders}";
            AverageOrderValueTextBlock.Text = $"Giá trị trung bình mỗi đơn: {averageOrderValue:N0} VND";

            // Chi tiết đơn hàng
            OrdersDataGrid.ItemsSource = bills;

            // Sản phẩm bán chạy
            var topProducts = _context.BillDetails
                .Where(bd => bd.Bill.BillDate >= startDate && bd.Bill.BillDate <= endDate)
                .GroupBy(bd => bd.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Title,
                    QuantitySold = g.Sum(bd => bd.Quantity),
                    Revenue = g.Sum(bd => bd.TotalPrice)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .ToList();

            TopProductsDataGrid.ItemsSource = topProducts;
        }
    }
}