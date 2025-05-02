using System.Security.Claims;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Patient.CancleAppointment;


public class CancleAppointmnetEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/v2/patient/appointment/assign/cancle/{id}", Handler);
    }

    private async Task<IResult> Handler(int id,HttpContext context, CourseWork2Context courseWork2Context)
    {
        
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await courseWork2Context.MedicalHelpRequests
            .Include(r => r.Status)
            .Include(r => r.Patient)
            .ThenInclude(r => r.Account)
            .FirstOrDefaultAsync(r => r.Id == id);

        
        if (result == null || result.Patient.Account.Id != userId)
        {
            return Results.NotFound();
        }
        
        var status = await courseWork2Context.HelpRequestStatuses.SingleOrDefaultAsync(s => s.Name == "CancelledByPatient");
        if (status != null) result.StatusId = status.Id;

        courseWork2Context.MedicalHelpRequests.Update(result);
        await courseWork2Context.SaveChangesAsync();
        
        
        return Results.Ok();
    }
}