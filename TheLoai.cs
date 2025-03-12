using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class TheLoai
{
    public int TheLoaiId { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
