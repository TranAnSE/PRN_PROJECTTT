using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class VoucherManagementPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public VoucherManagementPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadVoucherBlocks();
        }

        private void LoadVoucherBlocks()
        {
            VoucherBlocksDataGrid.ItemsSource = _context.BlockVouchers.ToList();
        }

        private void CreateVoucherBlock_Click(object sender, RoutedEventArgs e)
        {
            var createVoucherBlockWindow = new CreateVoucherBlockWindow();
            if (createVoucherBlockWindow.ShowDialog() == true)
            {
                LoadVoucherBlocks();
            }
        }

        private void ViewVoucherBlockDetails_Click(object sender, RoutedEventArgs e)
        {
            var voucherBlock = (sender as Button).DataContext as BlockVoucher;
            if (voucherBlock != null)
            {
                var voucherBlockDetailsWindow = new VoucherBlockDetailsWindow(voucherBlock.Id);
                voucherBlockDetailsWindow.ShowDialog();
            }
        }

        private void DeleteVoucherBlock_Click(object sender, RoutedEventArgs e)
        {
            var voucherBlock = (sender as Button).DataContext as BlockVoucher;
            if (voucherBlock != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa đợt voucher này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.BlockVouchers.Remove(voucherBlock);
                    _context.SaveChanges();
                    LoadVoucherBlocks();
                }
            }
        }
    }
}