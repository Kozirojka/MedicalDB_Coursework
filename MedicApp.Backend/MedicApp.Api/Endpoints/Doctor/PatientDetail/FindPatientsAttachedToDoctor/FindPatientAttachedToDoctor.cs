using System.Security.Claims;
using MedicApp.Api.Endpoints.Doctor.PatientDetail.FindPatientsAttachedToDoctor;
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
    
        var patients = Dbcontext.MedicalHelpRequests
            .Include(x => x.Patient)
            .ThenInclude(x => x.Account)
            .ThenInclude(x => x.Addresses)
            .Include(x => x.Doctor)
            .Where(x => x.Doctor.AccountId == userId)
            .Select(x => x.Patient)
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .ToList();

        return Results.Ok(patients.PatientToDto());
    }
}