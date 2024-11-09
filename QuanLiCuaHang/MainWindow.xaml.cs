using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class MainWindow : Window
    {
        private User _currentUser;

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;

            if (_currentUser.UserRole == "Admin")
            {
                AdminButton.Visibility = Visibility.Visible;
            }

            // Hiển thị trang chủ khi khởi động
            ShowHomePage();
        }

        private void ShowHomePage()
        {
            var homePage = new HomePage();
            homePage.NavigationRequested += HomePage_NavigationRequested;
            MainContent.Content = homePage;
        }

        private void HomePage_NavigationRequested(object sender, string destination)
        {
            switch (destination)
            {
                case "Sales":
                    SalesButton_Click(null, null);
                    break;
                case "Import":
                    ImportProductButton_Click(null, null);
                    break;
                case "Inventory":
                    InventoryButton_Click(null, null);
                    break;
                case "Report":
                    ReportButton_Click(null, null);
                    break;
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowHomePage();
        }

        private void ProductManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProductManagementPage();
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new InventoryPage();
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CheckoutPage();
        }

        private void ImportProductButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ImportProductPage();
        }

        private void CustomerManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CustomerManagementPage();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SalesReportPage();
        }

        private void VoucherManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VoucherManagementPage();
        }

        private void SalaryPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SalaryPaymentPage();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.UserRole == "Admin")
            {
                MainContent.Content = new EmployeeManagementPage();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.");
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsPage(_currentUser);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}