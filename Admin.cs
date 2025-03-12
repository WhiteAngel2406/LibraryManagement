using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<MuonSach> MuonSaches { get; set; } = new List<MuonSach>();

    public virtual ICollection<YeuCauMuonSach> YeuCauMuonSaches { get; set; } = new List<YeuCauMuonSach>();
}
