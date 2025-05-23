﻿using MediatR;
using MedicApp.Domain.Dto;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Extension;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Admin.Query.FindAppendingRequests;

public class FindAppendingRequestsQueryHandler : IRequestHandler<FindAppendingRequestsQuery, List<VisitResponceDto>>
{
    
    private readonly CourseWork2Context _dbContext;

    public FindAppendingRequestsQueryHandler(
        CourseWork2Context dbContext)
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
            .Include(v => v.Patient) 
            .ThenInclude(p => p.Account.Addresses) 
            .AsNoTracking()
            .Select(v => new VisitResponceDto
            {
                Id = v.Id,
                Description = v.Description,
                PatienId = v.PatientId,
                Address = v.Patient.Account.Addresses.FirstOrDefault().ToDto(),
            })
            .ToListAsync(cancellationToken);

        return listOfRequests;
        
    }
}


public class VisitResponceDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    
    public AddressDto Address { get; set; }
    public int PatienId { get; set; }
}

