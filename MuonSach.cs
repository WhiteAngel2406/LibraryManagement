using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class MuonSach
{
    public int MaMuon { get; set; }

    public int? MaSinhVien { get; set; }

    public int? SachId { get; set; }

    public DateOnly? NgayMuon { get; set; }

    public DateOnly? NgayTra { get; set; }

    public string? TrangThai { get; set; }

    public int? AdminId { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<LogMuonSach> LogMuonSaches { get; set; } = new List<LogMuonSach>();

    public virtual SinhVien? MaSinhVienNavigation { get; set; }

    public virtual Sach? Sach { get; set; }
}
