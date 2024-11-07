using System;
using System.Collections.Generic;

namespace ProjectQLNV.DuLieu;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string? HoTen { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? NgoaiNgu { get; set; }

    public decimal? Luong { get; set; }

    public string MaPb { get; set; } = null!;

    public virtual PhongBan MaPbNavigation { get; set; } = null!;
}
