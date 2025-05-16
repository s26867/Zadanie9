using System.ComponentModel.DataAnnotations;

namespace Zadanie9.Models;

public class PrescriptionMedicament
{
    [Key]
    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; }

    public int IdMedicament { get; set; }
    public Medicament Medicament { get; set; }

    public int Dose { get; set; }
    public string Description { get; set; }
}