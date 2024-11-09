using System;
using System.Collections.Generic;

namespace QuanLiCuaHang.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? Point { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
