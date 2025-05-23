﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using MedicApp.Domain.Dto.Responce;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Appointment.Command.AssignDoctorToVisit;

public class AssignDoctorToVisitCommandHandler(
    CourseWork2Context dbContext)
    : IRequestHandler<AssignDoctorToVisitCommand, AssignmentResult>
{
    public async Task<AssignmentResult> Handle(AssignDoctorToVisitCommand request, CancellationToken cancellationToken)
    {
        var medicalHelpRequest = dbContext.MedicalHelpRequests.FirstOrDefault(u => u.Id == request.VisitId);

        if (medicalHelpRequest == null)
        {
            Console.WriteLine("Visit with ID {VisitId} not found, request.VisitId");
            
            throw new NotFoundException($"Visit with ID {request.VisitId} not found");
        }

        var doctor = await dbContext.Accounts.FindAsync(request.DoctorId, cancellationToken);
        
        var role = dbContext.Roles.SingleOrDefault(u => u.Name == "Doctor");
        
        if (doctor == null || doctor.Role.Name != role?.Name)
        {
            throw new ValidationException("Invalid doctor ID or user is not a doctor"); 
            
        }


        var status = await dbContext.HelpRequestStatuses
            .SingleOrDefaultAsync(u => u.Name == "InProgress", cancellationToken: cancellationToken);
        

        
        medicalHelpRequest.StatusId = status.Id;
       
        var sceduleInterval = dbContext.ScheduleIntervals.SingleOrDefault(u => u.Id == request.SlotTimeId);
        
        
        medicalHelpRequest.ScheduleIntervalId = sceduleInterval.Id;
        
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        
        return new AssignmentResult
        {
            Success = true,
            Message = "Doctor successfully assigned to visit",
        };
        
    }
}
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

