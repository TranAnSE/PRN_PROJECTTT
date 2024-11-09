using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using QuanLiCuaHang.Models;
using System.IO;

namespace QuanLiCuaHang
{
    public partial class IncidentReportPage : UserControl
    {
        private readonly QlcuahangContext _context;
        private readonly User _currentUser;
        private string _imagePath;

        public IncidentReportPage(QlcuahangContext context, User currentUser)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        private void AttachImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _imagePath = openFileDialog.FileName;
                AttachedImage.Source = new BitmapImage(new Uri(_imagePath));
            }
        }

        private void SubmitReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tiêu đề và mô tả.");
                return;
            }

            var report = new Report
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Chưa xử lý",
                SubmittedAt = DateTime.Now,
                RepairCost = int.TryParse(RepairCostTextBox.Text, out int cost) ? cost : 0,
                StartDate = StartDatePicker.SelectedDate,
                FinishDate = FinishDatePicker.SelectedDate,
                StaffId = _currentUser.Id
            };

            if (!string.IsNullOrEmpty(_imagePath))
            {
                report.Image = File.ReadAllBytes(_imagePath);
            }

            _context.Reports.Add(report);
            _context.SaveChanges();

            MessageBox.Show("Báo cáo đã được gửi thành công!");
            ClearForm();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            StatusComboBox.SelectedIndex = -1;
            RepairCostTextBox.Clear();
            StartDatePicker.SelectedDate = null;
            FinishDatePicker.SelectedDate = null;
            AttachedImage.Source = null;
            _imagePath = null;
        }
    }
}