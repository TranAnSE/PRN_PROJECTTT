using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class ImportManagementPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public ImportManagementPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadImportData();
        }

        private void LoadImportData()
        {
            ImportDataGrid.ItemsSource = _context.InputInfos
                .Include(i => i.Supplier)
                .ToList();
        }

        private void AddNewBatch_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị màn hình thêm lô hàng mới
            var addBatchWindow = new AddBatchWindow();
            if (addBatchWindow.ShowDialog() == true)
            {
                LoadImportData(); // Tải lại dữ liệu sau khi thêm
            }
        }

        private void ViewBatchDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var inputInfo = button.DataContext as InputInfo;
            if (inputInfo != null)
            {
                // Hiển thị màn hình chi tiết lô hàng
                var batchDetailsWindow = new BatchDetailsWindow(inputInfo.Id);
                batchDetailsWindow.ShowDialog();
            }
        }
    }
}