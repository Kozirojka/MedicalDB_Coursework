using MediatR;
using MedicalVisits.Application.Admin.Queries.FindAppendingRequests;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Admin.Query.FindAppendingRequests;

public class FindAppendingRequestsQueryHandler : IRequestHandler<FindAppendingRequestsQuery, List<VisitResponceDto>>
{
    
    private readonly CourseWorkDbContext _dbContext;

    public FindAppendingRequestsQueryHandler(
        CourseWorkDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    
    /*Проблема полягає у тому, що EF Core не може відслідковувати власні (owned) сутності без їх власника.
      У вашому випадку Address є власною сутністю, оскільки її налаштовано через OwnsOne.
      Коли ви використовуєте запит Select і вибираєте лише частину даних (наприклад, v.Patient.Address), 
      EF Core не має інформації про власника (тобто Patient).
      Ця помилка виникає через те, що власні сутності (Address) не можуть існувати окремо від свого власника (Patient).
      Для вирішення проблеми є кілька підходів:*/
    public async Task<List<VisitResponceDto>> Handle(FindAppendingRequestsQuery requests, CancellationToken cancellationToken)
    {

        var listOfRequests = await _dbContext.MedicalHelpRequests
            .Where(p => p.Status.Name == "LookingForAssign")
            .Include(v => v.Patient) // Завантажуємо навігаційну властивість Patient
            .ThenInclude(p => p.Account.Addresses) // Завантажуємо Address з ApplicationUser
            .AsNoTracking()
            .Select(v => new VisitResponceDto
            {
                Id = v.Id,
                Description = v.Description,
                PatienId = v.PatientId
            })
            .ToListAsync(cancellationToken);

        return listOfRequests;
        
    }
}


public class VisitResponceDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    
    public Address Address { get; set; }
    public int PatienId { get; set; }
}

