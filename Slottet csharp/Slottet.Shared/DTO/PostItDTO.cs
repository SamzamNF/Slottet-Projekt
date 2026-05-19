namespace Slottet.Shared.DTO;

public class PostItDTO
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Payment { get; set; } = string.Empty;
    public string ShoppingDay { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Events { get; set; } = string.Empty;
    public string RelativesContact { get; set; } = string.Empty;

    // Til dummy data, hov, fy fy, dansk kommentar
    public ResidentDto? Resident { get; set; }
    public RiskDTO? Risk { get; set; }
    public StaffDto? LastEditedBy { get; set; }
    public List<MedicineDto> Medicines { get; set; } = new();
    public List<PnTimeDTO> PnTimes { get; set; } = new();
    public List<StaffDto> Staffs { get; set; } = new();
}