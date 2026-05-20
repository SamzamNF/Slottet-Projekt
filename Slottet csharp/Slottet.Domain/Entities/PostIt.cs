using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class PostIt
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Payment { get; set; } = string.Empty;
    public string ShoppingDay { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Events { get; set; } = string.Empty;
    public string RelativesContact { get; set; } = string.Empty;

    // Foreign keys
    public int StaffId { get; set; }
    public Staff? Staff { get; set; }
    public int ResidentId { get; set; }
    public Resident? Resident { get; set; }

    public Risk? Risk { get; set; }
    
    private readonly List<Medicine> _medicines = new List<Medicine>();
    public IReadOnlyCollection<Medicine> Medicines => _medicines.AsReadOnly();

    private readonly List<PnTime> _pnTimes = new List<PnTime>();
    public IReadOnlyCollection<PnTime> PnTimes => _pnTimes.AsReadOnly();

    private PostIt() { }

    // Service
    public static PostIt Create(
        DateTime date,
        string payment,
        string shoppingDay,
        string mood,
        string status,
        string events,
        string relativesContact
    )
    {
        if (date == default)
            throw new ArgumentException("Dato er påkrævet");
        if (string.IsNullOrWhiteSpace(payment))
            throw new ArgumentException("Betaling er påkrævet");
        if (string.IsNullOrWhiteSpace(shoppingDay))
            throw new ArgumentException("Indkøbsdag er påkrævet");
        if (string.IsNullOrWhiteSpace(mood))
            throw new ArgumentException("Humør er påkrævet");
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status er påkrævet");

        return new PostIt
        {
            Date = date,
            Payment = payment,
            ShoppingDay = shoppingDay,
            Mood = mood,
            Status = status,
            Events = events,
            RelativesContact = relativesContact,
        };
    }

    public void AddMedicine(Medicine medicine)
    {
        if (medicine == null)
            throw new ArgumentNullException(nameof(medicine));

        var medicineToAdd = Medicine.Create(medicine.ResidentId, medicine.StaffId, medicine.Description, medicine.TimeStamp);
        _medicines.Add(medicineToAdd);
    }

    public void AddPnTime(PnTime pnTime)
    {
        if (pnTime == null)
            throw new ArgumentNullException(nameof(pnTime));

        var pnTimeToAdd = PnTime.Create(pnTime.Time, pnTime.Description);
        _pnTimes.Add(pnTimeToAdd);
    }

    public void Update(
        DateTime date,
        string payment,
        string shoppingDay,
        string mood,
        string status,
        string events,
        string relativesContact
    )
    {
        if (date == default)
            throw new ArgumentException("Dato er påkrævet");
        if (string.IsNullOrWhiteSpace(payment))
            throw new ArgumentException("Betaling er påkrævet");
        if (string.IsNullOrWhiteSpace(shoppingDay))
            throw new ArgumentException("Indkøbsdag er påkrævet");
        if (string.IsNullOrWhiteSpace(mood))
            throw new ArgumentException("Humør er påkrævet");
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status er påkrævet");

        Date = date;
        Payment = payment;
        ShoppingDay = shoppingDay;
        Mood = mood;
        Status = status;
        Events = events;
        RelativesContact = relativesContact;
    }

    // Method to update all PnTimes of a PostIt, calls the Entity method to update each PnTime with validation
    public void UpdatePnTimes(List<(int Id, DateTime Time, string Description)> incomingPnTimes)
    {
        if (incomingPnTimes == null)
            throw new ArgumentNullException(nameof(incomingPnTimes));

        foreach (var incomingPn in incomingPnTimes)
        {
            var originalPn = _pnTimes.Find(p => p.Id == incomingPn.Id);

            if (originalPn == null)
                throw new InvalidOperationException($"PnTime med ID {incomingPn.Id} tilhører ikke denne PostIt.");

            originalPn.Update(incomingPn.Time, incomingPn.Description);
        }
    }

    // Method to update all Medicines of a PostIt, calls the Entity method to update each Medicine with validation
    public void UpdateMedicines(List<(int Id, string Description)> incomingMedicines, bool isAdmin = false)
    {
        if (incomingMedicines == null)
            throw new ArgumentNullException(nameof(incomingMedicines));

        foreach (var incomingMed in incomingMedicines)
        {
            var originalMed = _medicines.Find(m => m.Id == incomingMed.Id);

            if (originalMed == null)
                throw new InvalidOperationException($"Medicine med ID {incomingMed.Id} tilhører ikke denne PostIt.");

            originalMed.Update(incomingMed.Description, isAdmin);
        }
    }
}
