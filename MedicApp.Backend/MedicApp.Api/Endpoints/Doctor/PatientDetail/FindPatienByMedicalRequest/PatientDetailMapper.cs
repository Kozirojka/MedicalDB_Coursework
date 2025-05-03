namespace MedicApp.Api.Endpoints.Doctor.PatientDetail;

public static class PatientDetailMapper
{
    public static PatientDto PatientToDto(this Infrastructure.Models.Patient patient)
    {
        if (patient == null)
            return null;
            
        return new PatientDto
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
        };
    }
    
    
    public static PatientDto PatientForPage(this Infrastructure.Models.Patient patient)
    {
        if (patient == null)
            return null;
            
        return new PatientDto
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
        };
    }
}

public class PatientDto
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}

public class AddressDto
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string ApartmentNumber { get; set; }
}