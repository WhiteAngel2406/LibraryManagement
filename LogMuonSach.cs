using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class LogMuonSach
{
    public int LogId { get; set; }

    public int? MaMuon { get; set; }

    public DateOnly? NgayThucTra { get; set; }

    public string? GhiChu { get; set; }

    public virtual MuonSach? MaMuonNavigation { get; set; }
}
