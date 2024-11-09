using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class Consignment
{
    public int InputInfoId { get; set; }

    public string ProductId { get; set; } = null!;

    public int? Stock { get; set; }

    public DateOnly? ManufacturingDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public int? InputPrice { get; set; }

    public int? OutputPrice { get; set; }

    public double? Discount { get; set; }

    public int? InStock { get; set; }

    public virtual InputInfo InputInfo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
