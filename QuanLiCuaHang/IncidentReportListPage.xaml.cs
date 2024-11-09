using System.Linq;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class IncidentReportListPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public IncidentReportListPage(QlcuahangContext context)
        {
            InitializeComponent();
            _context = context;
            LoadReports();
        }

        private void LoadReports()
        {
            var reports = _context.Reports
                .Include(r => r.Staff)
                .OrderByDescending(r => r.SubmittedAt)
                .ToList();
            ReportsDataGrid.ItemsSource = reports;
        }

        private void ReportsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportsDataGrid.SelectedItem is Report selectedReport)
            {
                var detailWindow = new IncidentReportDetailWindow(selectedReport, _context);
                detailWindow.ShowDialog();
                LoadReports(); // Reload reports after closing detail window
            }
        }
    }
}