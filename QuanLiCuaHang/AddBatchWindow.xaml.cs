using System;
using System.Linq;
using System.Windows;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class AddBatchWindow : Window
    {
        private readonly QlcuahangContext _context;

        public AddBatchWindow()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            SupplierComboBox.ItemsSource = _context.Suppliers.ToList();
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (InputDatePicker.SelectedDate == null || SupplierComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            var newBatch = new InputInfo
            {
                InputDate = DateOnly.FromDateTime(InputDatePicker.SelectedDate.Value),
                SupplierId = SupplierComboBox.SelectedItem is Supplier supplier ? supplier.Id : 0,
                // Thêm các thông tin khác nếu cần
            };

            _context.InputInfos.Add(newBatch);
            _context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}