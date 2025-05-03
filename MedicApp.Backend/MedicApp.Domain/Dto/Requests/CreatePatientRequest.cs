namespace MedicApp.Domain.Dto.Requests;

public class CreatePatientRequest
{
    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;
    public string Password {get; set;} = null!;   
    public string Phonenumber { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    
    public AddressDto Address { get; set; } = null!;
}

public class CreateDoctorRequest
{
    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;
    public string Password {get; set;} = null!;   
    public string Phonenumber { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    public List<string> Specialization { get; set; } = null!;
    public List<string> Education { get; set; } = null!;
    
    public AddressDto Address { get; set; } = null!;
}
