using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class Product
{
    public string Barcode { get; set; } = null!;

    public string? Title { get; set; }

    public byte[]? Image { get; set; }

    public string? Type { get; set; }

    public string? ProductionSite { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();
}
