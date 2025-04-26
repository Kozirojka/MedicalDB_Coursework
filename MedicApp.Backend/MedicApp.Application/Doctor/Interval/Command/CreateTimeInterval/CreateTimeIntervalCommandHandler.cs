using MediatR;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Interval.Command.CreateTimeInterval
{
    public record CreateTimeIntervalCommand(int ScheduleId, int DoctorId, IntervalDto? IntervalDto) : IRequest<bool>;

    public class CreateTimeIntervalCommandHandler : IRequestHandler<CreateTimeIntervalCommand, bool>
    {
        private readonly CourseWorkDbContext _dbContext;

        public CreateTimeIntervalCommandHandler(CourseWorkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CreateTimeIntervalCommand request, CancellationToken cancellationToken)
        {
            if (request.IntervalDto == null)
            {
                return false;
            }

            var schedule = await _dbContext.Schedules
                .FirstOrDefaultAsync(s => s.Id == request.ScheduleId && s.DoctorId == request.DoctorId, cancellationToken);

            if (schedule == null)
            {
                return false; 
            }

            var isIntersecting = await _dbContext.ScheduleIntervals
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

            var newInterval = new ScheduleInterval
            {
                ScheduleId = request.ScheduleId,
                StartTime = request.IntervalDto.StartTime,
                EndTime = request.IntervalDto.EndTime,
                Schedule = schedule,
            };

            if (request.IntervalDto.MedicHelp != null)
            {
                var helpRequest = await _dbContext.MedicalHelpRequests
                    .FindAsync(request.IntervalDto.MedicHelp, cancellationToken);

                if (helpRequest != null)
                {
                    helpRequest.ScheduleIntervalId = newInterval.Id; 
                }
            }

            _dbContext.ScheduleIntervals.Add(newInterval);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true; 
        }
    }
}
