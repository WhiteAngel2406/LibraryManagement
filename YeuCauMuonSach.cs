using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class YeuCauMuonSach
{
    public int MaYeuCau { get; set; }

    public int? MaSinhVien { get; set; }

    public int? SachId { get; set; }

    public int? AdminId { get; set; }

    public DateOnly? NgayYeuCau { get; set; }

    public string? TrangThai { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual SinhVien? MaSinhVienNavigation { get; set; }

    public virtual Sach? Sach { get; set; }
}
