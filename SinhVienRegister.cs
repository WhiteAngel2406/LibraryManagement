using System;
using System.Collections.Generic;

namespace LibraryManagement_PRJ01.Models;

public partial class SinhVienRegister
{
    public int MaSinhVien { get; set; }

    public string Ten { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Lop { get; set; }
}
