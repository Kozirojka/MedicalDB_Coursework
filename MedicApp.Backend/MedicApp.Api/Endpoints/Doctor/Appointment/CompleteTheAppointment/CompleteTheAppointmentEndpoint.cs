using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.Appointment.CompleteTheAppointment;

public record DoctorComments(string doctorNotes, int patientAdequacy);

public class CompleteTheAppointmentEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/doctor/appointment/assign/complete/{id}", Handler);
    }

    private async Task<IResult> Handler([FromRoute] int id, [FromBody] DoctorComments request, HttpContext context, CourseWork2Context dbContext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        if (string.IsNullOrWhiteSpace(request.doctorNotes))
        {
            return Results.BadRequest("Doctor notes cannot be empty");
        }
        
        if (request.patientAdequacy < 1 || request.patientAdequacy > 10) 
        {
            return Results.BadRequest("Patient adequacy must be between 1 and 5");
        }
        
        try
        {
            var medicalRequest = await dbContext.MedicalHelpRequests
                .Include(r => r.Status)
                .Include(r => r.ScheduleInterval)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.ScheduleInterval.Id == id);
            
            if (medicalRequest == null)
            {
                return Results.NotFound("Medical help request not found");
            }
            
            var completedStatus = await dbContext.HelpRequestStatuses
                .FirstOrDefaultAsync(s => s.Name == "Completed");
                
            if (completedStatus == null)
            {
                return Results.Problem("Status 'Completed' not found in the database");
            }
            
            medicalRequest.StatusId = completedStatus.Id;
            
            // Add comment
            var comment = new Comment
            {
                Adequacy = request.patientAdequacy,
                AuthorId = userId,
                CommentText = request.doctorNotes,
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),

                HelpRequestId = medicalRequest.Id
            };
            
            // Initialize collection if null
            medicalRequest.Comments ??= new List<Comment>();
            medicalRequest.Comments.Add(comment);
            
            await dbContext.SaveChangesAsync();
            
            return Results.Ok();
        }
        catch (Exception ex)
        {
            // Log the exception here if you have a logging system
            return Results.Problem($"An error occurred while processing your request: {ex.Message}");
        }
    }
}