using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.PatientDetail.FindPatientAttachedToDoctor;

public class FindPatientAttachedToDoctor : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/doctor/patients", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, CourseWork2Context Dbcontext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
    
        var patient = Dbcontext.MedicalHelpRequests
            .Include(x => x.Patient)
            .ThenInclude(x => x.Account)
            .ThenInclude(x => x.Addresses)
            .Where(x => x.Doctor.AccountId == userId)
            .Select(x => x.Patient)
            .FirstOrDefault();;
        
        return Results.Ok(patient.PatientToDto());
    }
}