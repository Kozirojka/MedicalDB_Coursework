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

        var targetDate = DateOnly.FromDateTime(request.IntervalDto.EndTime);

        var schedule = await dbContext.Schedules
            .FirstOrDefaultAsync(s => s.Date == targetDate && s.DoctorId == request.DoctorId, cancellationToken);

        // Якщо не існує — створюємо новий
        if (schedule == null)
        {
            schedule = new Schedule
            {
                CreatedAt = DateTime.UtcNow,
                DoctorId = request.DoctorId,
                Date = targetDate
            };

            dbContext.Schedules.Add(schedule);
            await dbContext.SaveChangesAsync(cancellationToken); 
        }

        
        
        var timeStart = TimeOnly.FromDateTime(request.IntervalDto.StartTime);
        var timeEnd = TimeOnly.FromDateTime(request.IntervalDto.EndTime);
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

        // Створюємо інтервал
        var newInterval = new ScheduleInterval
        {
            ScheduleId = schedule.Id,
            StartTime = timeStart,
            EndTime = timeEnd,
            Schedule = schedule
        };

        dbContext.ScheduleIntervals.Add(newInterval);
        await dbContext.SaveChangesAsync(cancellationToken);
    
        //ось тут ми перевіряємо чи є у запиті інформацію про appointment
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