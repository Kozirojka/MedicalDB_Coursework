using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Patient.GetDoctorInformation;

public class GetDoctorInformation : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/doctor/info/{id}", Handler);
    }

    private async Task<IResult> Handler(int id, HttpContext context, CourseWork2Context dbContext)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        
        var doctorInfo = await dbContext.Doctors.Include(u => u.Account).SingleOrDefaultAsync(x => x.Id == id);
        
        
        return Results.Ok(doctorInfo.DoctorInfoDto());
    }
}