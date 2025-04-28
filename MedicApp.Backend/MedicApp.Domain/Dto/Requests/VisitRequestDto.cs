namespace MedicApp.Domain.Dto.Requests;

public class VisitRequestDto
{
    public string description { get; set; }
}


public class MedicineNeeded {
    public int Id {get;set;}
    public int Quantity {get;set;}
}