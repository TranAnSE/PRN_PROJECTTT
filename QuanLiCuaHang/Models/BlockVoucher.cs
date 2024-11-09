using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class BlockVoucher
{
    public int Id { get; set; }

    public string? ReleaseName { get; set; }

    public int? TypeVoucher { get; set; }

    public int? ParValue { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? FinishDate { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
