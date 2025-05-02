using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.Appointment.CompleteTheAppointment;

public class CompleteTheAppointmentEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/doctor/appointment/assign/complete/{id}", Handler);
    }

    private async Task<IResult> Handler(int id, HttpContext context, CourseWork2Context dbContext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        var patientId = dbContext.Patients
            .Include(a => a.Account)
            .SingleOrDefault(a => a.Account.Id == userId);

        var medicalRequest = await dbContext.MedicalHelpRequests
            .Include(r => r.Status)
            .SingleOrDefaultAsync(u => u.Id == id);
        
        if (medicalRequest == null || medicalRequest.PatientId != patientId.Id)
        {
            return Results.NotFound();
        }
        
        medicalRequest.Status.Name = "Completed";

        await dbContext.SaveChangesAsync();
        
        return Results.Ok();
    }
}