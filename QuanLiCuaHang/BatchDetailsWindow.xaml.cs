using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class BatchDetailsWindow : Window
    {
        private readonly QlcuahangContext _context;
        private readonly int _batchId;

        public BatchDetailsWindow(int batchId)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _batchId = batchId;
            LoadBatchDetails();
        }

        private void LoadBatchDetails()
        {
            var batch = _context.InputInfos
                .Include(i => i.Supplier)
                .Include(i => i.User)
                .FirstOrDefault(i => i.Id == _batchId);

            if (batch != null)
            {
                BatchInfoTextBlock.Text = $"Mã lô: {batch.Id}\n" +
                                          $"Ngày nhập: {batch.InputDate:dd/MM/yyyy}\n" +
                                          $"Nhà cung cấp: {batch.Supplier.Name}\n" +
                                          $"Người nhập: {batch.User.Name}";

                var products = _context.Consignments
                    .Include(c => c.Product)
                    .Where(c => c.InputInfoId == _batchId)
                    .ToList();

                ProductsDataGrid.ItemsSource = products;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin lô hàng.");
                Close();
            }
        }
    }
}