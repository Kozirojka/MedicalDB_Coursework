// IntervalDto.cs - No changes needed here

using MediatR;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Interval.Command.CreateTimeInterval;

public record CreateTimeIntervalCommand(int DoctorId, IntervalDto? IntervalDto) : IRequest<bool>;

public class CreateTimeIntervalCommandHandler(CourseWork2Context dbContext, IMediator mediator)
    : IRequestHandler<CreateTimeIntervalCommand, bool>
{
    public async Task<bool> Handle(CreateTimeIntervalCommand request, CancellationToken cancellationToken)
    {
        if (request.IntervalDto == null)
            return false;

        var localStartTime = DateTime.SpecifyKind(request.IntervalDto.StartTime, DateTimeKind.Unspecified);
        var localEndTime = DateTime.SpecifyKind(request.IntervalDto.EndTime, DateTimeKind.Unspecified);
        
        var targetDate = DateOnly.FromDateTime(localEndTime);
        var doctor = await dbContext.Doctors.SingleOrDefaultAsync(u => u.AccountId == request.DoctorId);
        
        var schedule = await dbContext.Schedules
            .FirstOrDefaultAsync(s => s.Date == targetDate && s.DoctorId == doctor.Id, cancellationToken);
        
        // Якщо не існує — створюємо новий
        if (schedule == null)
        {
            schedule = new Schedule
            {
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), // не DateTime.UtcNow!
                DoctorId = doctor.Id,
                Date = targetDate
            };
        
            dbContext.Schedules.Add(schedule);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        var timeStart = TimeOnly.FromDateTime(localStartTime);
        var timeEnd = TimeOnly.FromDateTime(localEndTime);
        
        var isIntersecting = await dbContext.ScheduleIntervals
            .AnyAsync(u =>
                    u.ScheduleId == schedule.Id &&
                    (
                        (timeStart < u.EndTime && timeStart >= u.StartTime) ||
                        (timeEnd > u.StartTime && timeEnd <= u.EndTime) ||
                        (timeStart <= u.StartTime && timeEnd >= u.EndTime)
                    ),
                cancellationToken);
        
        if (isIntersecting)
            return false;
        
        var newInterval = new ScheduleInterval
        {
            ScheduleId = schedule.Id,
            StartTime = timeStart,
            EndTime = timeEnd,
            Schedule = schedule
        };
        
        dbContext.ScheduleIntervals.Add(newInterval);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        if (request.IntervalDto.MedicHelp != null)
        {
            var helpRequest = await dbContext.MedicalHelpRequests
                .FindAsync([request.IntervalDto.MedicHelp.Value], cancellationToken);
        
            if (helpRequest != null)
            {
                helpRequest.ScheduleIntervalId = newInterval.Id;
            }
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return true;

    }
}