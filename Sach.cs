using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class Sach
{
    public int SachId { get; set; }

    public int? TheLoaiId { get; set; }

    public string Title { get; set; } = null!;

    public string? NoiDung { get; set; }

    public string? TacGia { get; set; }

    public int? SoLuong { get; set; }

    public int? NamXuatBan { get; set; }

    public string? NhaXuatBan { get; set; }

    public virtual ICollection<MuonSach> MuonSaches { get; set; } = new List<MuonSach>();

    public virtual TheLoai? TheLoai { get; set; }

    public virtual ICollection<YeuCauMuonSach> YeuCauMuonSaches { get; set; } = new List<YeuCauMuonSach>();
}
