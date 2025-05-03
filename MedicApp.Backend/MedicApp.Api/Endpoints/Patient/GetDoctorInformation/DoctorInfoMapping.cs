namespace MedicApp.Api.Endpoints.Patient.GetDoctorInformation;

public static class DoctorInfoMapping
{
    public static DoctorInfo DoctorInfoDto(this Infrastructure.Models.Doctor? doctor)
    {
         return new DoctorInfo()
        {
            FullName = doctor.Account.Firstname + " " + doctor.Account.Lastname,
            Phonenumber = doctor.Account.Phonenumber,
            Email = doctor.Account.Email,
            specializations = doctor.Specializations.Select(x => x.Name).ToList(),
            educations = doctor.Educations.Select(x => x.Name).ToList()
        };
    }
}


public class DoctorInfo
{
     public string FullName { get; set; } = null!;
     public string Phonenumber { get; set; } = null!;
     public string Email { get; set; } = null!;
     public List<string> specializations { get; set; } = new List<string>();
     public List<string> educations { get; set; } = new List<string>();
}


