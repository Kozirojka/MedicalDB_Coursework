using MedicApp.Api.Endpoints.Doctor.PatientDetail.FindPatienByMedicalRequest;

namespace MedicApp.Api.Endpoints.Doctor.PatientDetail.FindPatientsAttachedToDoctor;


public static class PatientsDetailMapper
{
    public static List<PatientDto> PatientToDto(this List<Infrastructure.Models.Patient> patients)
    {
        if (patients == null || !patients.Any())
            return new List<PatientDto>();
            
        return patients.Select(patient => new PatientDto
        {
            Id = patient.Id,
            AccountId = patient.AccountId,
            FirstName = patient.Account?.Firstname,
            LastName = patient.Account?.Lastname,
            PhoneNumber = patient.Account?.Phonenumber,
            Email = patient.Account?.Email,
            Addresses = patient.Account?.Addresses.Select(a => new AddressDto
            {
                Country = a.Country,
                City = a.City,
                Street = a.Street,
                BuildingNumber = a.Building,
                ApartmentNumber = a.Appartaments,
            }).ToList()
        }).ToList();
    }
    
    public static List<PatientDto> PatientsToDto(this List<Infrastructure.Models.Patient> patients)
    {
        if (patients == null || !patients.Any())
            return new List<PatientDto>();
            
        return patients.Select(patient => patient.PatientToDto()).Where(dto => dto != null).ToList();
    }
}