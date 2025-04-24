namespace MedicApp.Domain.Dto.Responce;



public class DoctorProfileWithDistance
{
    public double Distance { get; set; } 
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Specialization { get; set; } 
}