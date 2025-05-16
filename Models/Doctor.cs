namespace Zadanie9.Models;
using System.ComponentModel.DataAnnotations;

public class Doctor
{
    [Key]
    public int IdDoctor { get; set; } 

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

}