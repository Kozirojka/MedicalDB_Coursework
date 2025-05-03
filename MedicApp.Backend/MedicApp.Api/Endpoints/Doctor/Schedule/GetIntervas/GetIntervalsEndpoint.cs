using System.Security.Claims;
using MediatR;
using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Doctor.Schedule.GetIntervas;

public class GetIntervalsEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/schedule/intervas", GetIntervas);
    }

    private async Task<IResult> GetIntervas(HttpContext context, CourseWork2Context dbContext)
    {
            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }

        var doctor = await dbContext.Doctors.SingleOrDefaultAsync(x => x.AccountId == userId);
        
        
        var results = dbContext.Schedules
            .Include(u => u.ScheduleIntervals)
            .ThenInclude(si => si.MedicalHelpRequests)
            .ThenInclude(si => si.Status)
            .Where(u => u.Date > DateOnly.FromDateTime(DateTime.Today))
            .Where(u => doctor != null && u.DoctorId == doctor.Id)
            .ToList();
        
        var scheduleDtos = ScheduleMapper.MapSchedules(results);
        
        return Results.Ok(scheduleDtos);
    }
}