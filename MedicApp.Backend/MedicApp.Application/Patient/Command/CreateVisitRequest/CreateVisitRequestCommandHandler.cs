using MediatR;
using MedicApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Patient.Command.CreateVisitRequest;

public class CreateVisitRequestCommandHandler : IRequestHandler<CreateVisitRequestCommand, CreateVisitRequestResponse>
{
    private readonly CourseWorkDbContext _context;

    public CreateVisitRequestCommandHandler(
        CourseWorkDbContext context)
    {
        _context = context;
    }

    public async Task<CreateVisitRequestResponse> Handle(
        CreateVisitRequestCommand request, 
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients.SingleOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken: cancellationToken);
        if (patient == null)
        {
            throw new ApplicationException("Patient not found");
        }

        var status = await _context.HelpRequestStatuses.SingleOrDefaultAsync(u => u.Name == "LookingForAssign", cancellationToken: cancellationToken);
        var help_request = new MedicalHelpRequest()
        {
            CreateAt = DateTime.Now,
            Description = request.Description, 
            StatusId = status.Id
        };
        
        await _context.MedicalHelpRequests.AddAsync(help_request, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        
        if (result)
        {
            return new CreateVisitRequestResponse
            {
                RequestId = help_request.Id,
                Message = "Appointment request created successfully"
            };
        }
        else
        {
            return new CreateVisitRequestResponse
            {
                Message = "Appointment request created successfully"
            };
        }
    }
}