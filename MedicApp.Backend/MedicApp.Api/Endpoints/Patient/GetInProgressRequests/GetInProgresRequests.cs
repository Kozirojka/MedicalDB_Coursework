using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Patient.GetInProgressRequests;

/// <summary>
/// Endpoint to retrieve a patient's in-progress medical visit requests
/// </summary>
public class GetPatientVisitRequestsEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/patient/visit-requests", HandleGetVisitRequestsAsync);
    }

    public async Task<IResult> HandleGetVisitRequestsAsync(HttpContext context, CourseWork2Context dbContext)
    {
        int? userId = GetAuthenticatedUserId(context);
        if (!userId.HasValue)
        {
            return Results.Unauthorized();
        }
        
        var patient = await dbContext.Patients.SingleOrDefaultAsync(x => x.AccountId == userId);
        if (patient == null)
        {
            return Results.NotFound("Patient not found for this user account");
        }
        
            
            
            
        var visitRequests = await dbContext.MedicalHelpRequests
            .Include(x => x.Status)
            .Include(x => x.ScheduleInterval)
            .ThenInclude(si => si.Schedule)  // Include the Schedule that contains the day
            .Include(x => x.Doctor)
            .ThenInclude(d => d.Account)
            .Where(x => x.PatientId == patient.Id)
            .ToListAsync();

        
        
        return Results.Ok(visitRequests.MapToDto());
    }
    
    private static int? GetAuthenticatedUserId(HttpContext context)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        
        return null;
    }
}