using MediatR;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Interval.Command.CreateScheduleDay;

public record CreateScheduleDayCommand(int DoctorId, DateOnly Date) : IRequest<bool>;

public class CreateScheduleDayCommandHandler(CourseWork2Context context)
    : IRequestHandler<CreateScheduleDayCommand, bool>
{
    public async Task<bool> Handle(CreateScheduleDayCommand request, CancellationToken cancellationToken)
    {
        var doctorExists = await context.Accounts
            .AnyAsync(d => d.Id == request.DoctorId, cancellationToken);

        if (!doctorExists)
        {
            return false;
        }

        var existingSchedule = await context.Schedules
            .SingleOrDefaultAsync(
                s => s.DoctorId == request.DoctorId && s.Date == request.Date,
                cancellationToken
            );

        if (existingSchedule != null)
        {
            return false;
        }

        var newSchedule = new Schedule
        {
            DoctorId = request.DoctorId,
            Date = request.Date,
            CreatedAt = DateTime.UtcNow 
        };

        context.Schedules.Add(newSchedule);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
