namespace MedicApp.Domain.Dto.Responce;

public class VisitRequestResponce
{
    public int PatientId { get; set; }
    public string Description { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public int Id { get; set; }
    public AddressDto Address { get; set; }
    
    public string Status { get; set; }
}
