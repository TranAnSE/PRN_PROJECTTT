using System;
using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class CreateVoucherBlockWindow : Window
    {
        private readonly QlcuahangContext _context;

        public CreateVoucherBlockWindow()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
        }

        private void CreateVoucherBlock_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReleaseNameTextBox.Text) ||
                TypeVoucherComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(ParValueTextBox.Text) ||
                StartDatePicker.SelectedDate == null ||
                FinishDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            var newBlock = new BlockVoucher
            {
                ReleaseName = ReleaseNameTextBox.Text,
                TypeVoucher = TypeVoucherComboBox.SelectedIndex,
                ParValue = int.Parse(ParValueTextBox.Text),
                StartDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value),
                FinishDate = DateOnly.FromDateTime(FinishDatePicker.SelectedDate.Value)
            };

            _context.BlockVouchers.Add(newBlock);
            _context.SaveChanges();

            int quantity = int.Parse(QuantityTextBox.Text);
            for (int i = 0; i < quantity; i++)
            {
                var voucher = new Voucher
                {
                    Code = GenerateVoucherCode(),
                    Status = 0, // Assuming 0 means unused
                    BlockId = newBlock.Id
                };
                _context.Vouchers.Add(voucher);
            }

            _context.SaveChanges();

            MessageBox.Show("Đã tạo đợt voucher thành công!");
            DialogResult = true;
            Close();
        }

        private string GenerateVoucherCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}