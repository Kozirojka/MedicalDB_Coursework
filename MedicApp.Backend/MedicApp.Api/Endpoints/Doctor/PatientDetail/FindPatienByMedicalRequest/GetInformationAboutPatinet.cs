using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.PatientDetail;



public class GetInformationAboutPatinet : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v2/doctor/medical-request/patient/{id}", Handler);
    }

    private async Task<IResult> Handler(int id, HttpContext context, CourseWork2Context dbContext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        var medicalrequest = dbContext.MedicalHelpRequests
            .Include(u => u.Patient)
            .ThenInclude(u => u.Account)
            .ThenInclude(u => u.Addresses)
            .SingleOrDefault(x => x.Id == id);
        
        var patient = medicalrequest?.Patient;
        
        return Results.Ok(patient.PatientToDto());
        
        
    }
}