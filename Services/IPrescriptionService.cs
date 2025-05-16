using Zadanie9.DTOs;
using Zadanie9.Models;

namespace Zadanie9.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(CreatePrescriptionRequest request);
    Task<Patient?> GetPatientDetailsAsync(int idPatient);
}