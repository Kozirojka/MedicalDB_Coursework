using MediatR;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Interval.Command.CreateTimeInterval
{
    public record CreateTimeIntervalCommand(int ScheduleId, int DoctorId, IntervalDto? IntervalDto) : IRequest<bool>;

    public class CreateTimeIntervalCommandHandler(CourseWorkDbContext dbContext)
        : IRequestHandler<CreateTimeIntervalCommand, bool>
    {
        public async Task<bool> Handle(CreateTimeIntervalCommand request, CancellationToken cancellationToken)
        {
            var schedule = await dbContext.Schedules
                .FirstOrDefaultAsync(s => s.Id == request.ScheduleId && s.DoctorId == request.DoctorId, cancellationToken);

            if (schedule == null)
            {
                return false;
            }

            var isIntersecting = await dbContext.ScheduleIntervals
                .AnyAsync(u =>
                    u.ScheduleId == request.ScheduleId &&
                    (
                        (request.IntervalDto.StartTime < u.EndTime && request.IntervalDto.StartTime >= u.StartTime) ||
                        (request.IntervalDto.EndTime > u.StartTime && request.IntervalDto.EndTime <= u.EndTime) ||
                        (request.IntervalDto.StartTime <= u.StartTime && request.IntervalDto.EndTime >= u.EndTime)
                    ), cancellationToken);

            if (isIntersecting)
            {
                return false; 
            }

            
            if (request.IntervalDto == null)
            {
                return false; 
            }

            var newInterval = new ScheduleInterval
            {
                ScheduleId = request.ScheduleId,
                StartTime = request.IntervalDto.StartTime,
                EndTime = request.IntervalDto.EndTime,
                Schedule = schedule 
            };

           
            dbContext.ScheduleIntervals.Add(newInterval);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true; 
        }
    }
}
