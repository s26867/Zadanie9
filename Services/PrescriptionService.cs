using Microsoft.EntityFrameworkCore;
using Zadanie9.Data;
using Zadanie9.DTOs;
using Zadanie9.Models;

namespace Zadanie9.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly MyDbContext _context;

    public PrescriptionService(MyDbContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(CreatePrescriptionRequest request)
    {
        if (request.Medicaments.Count > 10)
            throw new ArgumentException("Recepta może zawierac maksymalnie 10 lekow");

        if (request.DueDate < request.Date)
            throw new ArgumentException("DueDate nie moze byc wczesniejszy niz Date");

        foreach (var m in request.Medicaments)
        {
            var exists = await _context.Medicaments.AnyAsync(x => x.IdMedicament == m.IdMedicament);
            if (!exists)
                throw new ArgumentException($"Lek ID: {m.IdMedicament} nie istnieje");
        }

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.FirstName == request.Patient.FirstName &&
                                      p.LastName == request.Patient.LastName &&
                                      p.Birthdate == request.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                Birthdate = request.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdDoctor = request.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Description = m.Description
            }).ToList()
        };

        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<Patient?> GetPatientDetailsAsync(int idPatient)
    {
        return await _context.Patients
            .Include(p => p.Prescriptions.OrderBy(p => p.DueDate))
                .ThenInclude(p => p.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);
    }
}
