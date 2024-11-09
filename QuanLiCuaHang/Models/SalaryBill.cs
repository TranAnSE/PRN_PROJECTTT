using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class SalaryBill
{
    public int Id { get; set; }

    public DateOnly? SalaryBillDate { get; set; }

    public int? UserId { get; set; }

    public int? TotalMoney { get; set; }

    public virtual User? User { get; set; }
}
