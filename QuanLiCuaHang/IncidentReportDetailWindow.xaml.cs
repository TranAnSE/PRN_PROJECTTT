using System.Windows;
using System.Windows.Controls;
using System.Linq;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class IncidentReportDetailWindow : Window
    {
        private readonly Report _report;
        private readonly QlcuahangContext _context;
        private readonly IncidentReportViewModel _viewModel;

        public IncidentReportDetailWindow(Report report, QlcuahangContext context)
        {
            InitializeComponent();
            _report = report;
            _context = context;
            _viewModel = new IncidentReportViewModel(report);
            DataContext = _viewModel;

            StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _report.Status);
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                _report.Status = selectedItem.Content.ToString();
                _context.SaveChanges();
                MessageBox.Show("Trạng thái đã được cập nhật.");
                Close();
            }
        }
    }
}