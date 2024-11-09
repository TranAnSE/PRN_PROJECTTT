using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class Report
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }

    public int? RepairCost { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public int StaffId { get; set; }

    public byte[]? Image { get; set; }

    public virtual User Staff { get; set; } = null!;
}
