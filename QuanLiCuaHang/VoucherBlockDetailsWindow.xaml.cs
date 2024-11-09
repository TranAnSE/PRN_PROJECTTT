using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class VoucherBlockDetailsWindow : Window
    {
        private readonly QlcuahangContext _context;
        private readonly int _blockId;

        public VoucherBlockDetailsWindow(int blockId)
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            _blockId = blockId;
            LoadBlockDetails();
        }

        private void LoadBlockDetails()
        {
            var block = _context.BlockVouchers
                .FirstOrDefault(b => b.Id == _blockId);

            if (block != null)
            {
                BlockInfoTextBlock.Text = $"Mã đợt: {block.Id}\n" +
                                          $"Tên đợt: {block.ReleaseName}\n" +
                                          $"Loại voucher: {(block.TypeVoucher == 0 ? "Giảm giá cố định" : "Giảm giá theo phần trăm")}\n" +
                                          $"Giá trị: {block.ParValue}\n" +
                                          $"Ngày bắt đầu: {block.StartDate:dd/MM/yyyy}\n" +
                                          $"Ngày kết thúc: {block.FinishDate:dd/MM/yyyy}";

                var vouchers = _context.Vouchers
                    .Where(v => v.BlockId == _blockId)
                    .ToList();

                VouchersDataGrid.ItemsSource = vouchers;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin đợt voucher.");
                Close();
            }
        }
    }
}