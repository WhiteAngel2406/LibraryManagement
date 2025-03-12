using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class SinhVien
{
    public int MaSinhVien { get; set; }

    public string Ten { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Lop { get; set; }

    public virtual ICollection<MuonSach> MuonSaches { get; set; } = new List<MuonSach>();

    public virtual ICollection<YeuCauMuonSach> YeuCauMuonSaches { get; set; } = new List<YeuCauMuonSach>();
}
