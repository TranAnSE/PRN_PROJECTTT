using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class Bill
{
    public int Id { get; set; }

    public DateTime? BillDate { get; set; }

    public int? CustomerId { get; set; }

    public int? UserId { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual Customer? Customer { get; set; }

    public virtual User? User { get; set; }
}
