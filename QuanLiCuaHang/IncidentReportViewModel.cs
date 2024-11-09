using System;
using System.Windows.Media.Imaging;
using QuanLiCuaHang.Models;

public class IncidentReportViewModel
{
    private readonly Report _report;

    public IncidentReportViewModel(Report report)
    {
        _report = report;
    }

    public string Title => _report.Title;
    public string Description => _report.Description;
    public DateTime? SubmittedAt => _report.SubmittedAt;
    public string StaffName => _report.Staff?.Name;
    public int? RepairCost => _report.RepairCost;
    public DateTime? StartDate => _report.StartDate;
    public DateTime? FinishDate => _report.FinishDate;
    public string Status => _report.Status;

    public BitmapImage ImageSource
    {
        get
        {
            if (_report.Image != null && _report.Image.Length > 0)
            {
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(_report.Image))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            return null;
        }
    }
}