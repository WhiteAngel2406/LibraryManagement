using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class ThongBao
{
    public int ThongBaoId { get; set; }

    public string? ThongBaoCate { get; set; }

    public string? ThongBaoName { get; set; }

    public string? NoiDung { get; set; }
}
