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

    public int? StaffId { get; set; }
    public int StaffDepartmentId { get; set; }
    public int ResidentId { get; set; }
    public string? StaffName { get; set; }
    public string? ResidentInitials { get; set; }
    public int ResidentDepartmentId { get; set; }
    public int? RiskId { get; set; }
    public string RiskAssessment { get; set; } = string.Empty;

    // Til dummy data, hov, fy fy, dansk kommentar
    public StaffDto? LastEditedBy { get; set; }
    public List<MedicineDto> Medicines { get; set; } = new();
    public List<PnTimeDTO> PnTimes { get; set; } = new();
}