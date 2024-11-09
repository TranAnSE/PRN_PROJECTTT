using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using QuanLiCuaHang.Models;
using System.Linq;

namespace QuanLiCuaHang
{
    public partial class HomePage : UserControl
    {
        private readonly QlcuahangContext _context;
        private DispatcherTimer _timer;

        // Định nghĩa một delegate cho event
        public delegate void NavigationRequestedEventHandler(object sender, string destination);

        // Định nghĩa event
        public event NavigationRequestedEventHandler NavigationRequested;

        public HomePage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadData();
            StartClock();
        }

        private void LoadData()
        {
            // Tổng doanh thu
            var totalRevenue = _context.Bills.Sum(b => b.Price);
            TotalRevenueTextBlock.Text = $"{totalRevenue:N0} VND";

            // Số đơn hàng
            var totalOrders = _context.Bills.Count();
            TotalOrdersTextBlock.Text = totalOrders.ToString();

            // Sản phẩm tồn kho
            var totalProducts = _context.Consignments.Sum(c => c.InStock);
            TotalProductsTextBlock.Text = totalProducts.ToString();

            // Số lượng khách hàng
            var totalCustomers = _context.Customers.Count();
            TotalCustomersTextBlock.Text = totalCustomers.ToString();
        }

        private void StartClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }

        private void QuickAccess_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string destination = button.Tag.ToString();
                // Kích hoạt event để thông báo MainWindow
                NavigationRequested?.Invoke(this, destination);
            }
        }
    }
}