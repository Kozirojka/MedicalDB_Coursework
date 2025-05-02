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
        };
    }
}


public class DoctorInfo
{
     public string FullName { get; set; } = null!;
     public string Phonenumber { get; set; } = null!;
     public string Email { get; set; } = null!;

}


