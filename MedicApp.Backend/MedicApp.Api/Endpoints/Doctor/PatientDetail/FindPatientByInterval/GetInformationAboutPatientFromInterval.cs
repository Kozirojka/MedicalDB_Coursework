using System.Security.Claims;
using MedicApp.Api.Endpoints.Doctor.PatientDetail.FindPatienByMedicalRequest;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.PatientDetail.FindPatientByInterval;

public class GetInformationAboutPatientFromInterval : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/patinet/interval/{id}", Handler);

    }

    private async Task<IResult> Handler(int id, HttpContext context, CourseWork2Context dbContext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }

        var visitRequsts = await dbContext.MedicalHelpRequests
            .Include(x => x.Patient)
            .ThenInclude(x => x.Account)
            .ThenInclude(x => x.Addresses)
            .SingleOrDefaultAsync(x => x.ScheduleIntervalId == id);
        
        var patient = visitRequsts?.Patient;
        
        if (patient is null)
        {
            return Results.NotFound();
        }
        
        return Results.Ok(patient.PatientToDto());
    }
}