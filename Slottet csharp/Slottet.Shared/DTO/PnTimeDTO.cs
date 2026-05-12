using System;

namespace Slottet.Shared.DTO;

public class PnTimeDTO
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string Description { get; set; } = string.Empty;
}
