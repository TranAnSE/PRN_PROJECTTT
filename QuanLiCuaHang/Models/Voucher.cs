using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class Voucher
{
    public string Code { get; set; } = null!;

    public int? Status { get; set; }

    public int? BlockId { get; set; }

    public virtual BlockVoucher? Block { get; set; }
}
