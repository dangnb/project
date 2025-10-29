using System.ComponentModel;

namespace MNG.Domain.Enums;

[Flags]
public enum ActionType
{
    [Description("View")]
    Xem = 1,
    [Description("Add/Delete")]
    ThemXoa = 2,
    [Description("Edit")]
    Sua = 4,
    [Description("Active")]
    KichHoat = 8,
    [Description("Dilivery/Revoke")]
    GiaoThuHoi = 16,
    [Description("Import")]
    Nhap = 32,
    [Description("Approve")]
    Duyet = 64,
    [Description("Send")]
    Gui = 128,
    [Description("Lock")]
    Khoa = 256,
}
