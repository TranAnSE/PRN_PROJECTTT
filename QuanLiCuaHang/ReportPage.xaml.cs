using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class ReportPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public ReportPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
        }

        private void ReportTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ReportTypeComboBox.SelectedIndex)
            {
                case 0:
                    LoadDailyRevenueReport();
                    break;
                case 1:
                    LoadBestSellingProductsReport();
                    break;
                case 2:
                    LoadTopEmployeesReport();
                    break;
            }
        }

        private void LoadDailyRevenueReport()
        {
            var report = _context.Bills
                   .GroupBy(b => b.BillDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(b => b.Price)
                })
                .OrderByDescending(r => r.Date)
                .ToList();

            ReportDataGrid.ItemsSource = report;
        }

        private void LoadBestSellingProductsReport()
        {
            var report = _context.BillDetails
                .GroupBy(bd => bd.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Title,
                    TotalQuantity = g.Sum(bd => bd.Quantity),
                    TotalRevenue = g.Sum(bd => bd.TotalPrice)
                })
                .OrderByDescending(r => r.TotalQuantity)
                .Take(10)
                .ToList();

            ReportDataGrid.ItemsSource = report;
        }

        private void LoadTopEmployeesReport()
        {
            var report = _context.Bills
                .GroupBy(b => b.UserId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    EmployeeName = g.First().User.Name,
                    TotalSales = g.Sum(b => b.Price),
                    TotalBills = g.Count()
                })
                .OrderByDescending(r => r.TotalSales)
                .Take(5)
                .ToList();

            ReportDataGrid.ItemsSource = report;
        }
    }
}