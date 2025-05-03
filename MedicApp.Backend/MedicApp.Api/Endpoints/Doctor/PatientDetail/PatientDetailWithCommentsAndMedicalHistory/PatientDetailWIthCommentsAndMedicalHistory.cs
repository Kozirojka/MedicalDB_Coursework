using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.PatientDetail.PatientDetailWithCommentsAndMedicalHistory;

public class PatientDetailWIthCommentsAndMedicalHistory : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/doctor/patient/history", Handler);
    }

    private async Task<IResult> Handler(int id, HttpContext context, CourseWork2Context dbContext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }


        var patientHistory = await dbContext.Patients
            .Where(p => p.Id == id) 
            .Include(x => x.Account)
            .ThenInclude(x => x.Addresses)
            .Include(x => x.MedicalHelpRequests)
            .ThenInclude(x => x.Comments)
            .ThenInclude(x => x.Author)
            .Include(x => x.MedicalHelpRequests)
            .ThenInclude(x => x.Status)
            .FirstOrDefaultAsync();

        return Results.Ok(patientHistory.PatientHistiryToDto());
    }
}