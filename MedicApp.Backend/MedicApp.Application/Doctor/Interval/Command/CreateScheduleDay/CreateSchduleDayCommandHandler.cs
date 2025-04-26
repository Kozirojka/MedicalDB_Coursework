using MediatR;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Interval.Command.CreateScheduleDay;

public record CreateScheduleDayCommand(int DoctorId, DateOnly Date) : IRequest<bool>;

public class CreateScheduleDayCommandHandler : IRequestHandler<CreateScheduleDayCommand, bool>
{
    private readonly CourseWorkDbContext _context;

    public CreateScheduleDayCommandHandler(CourseWorkDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateScheduleDayCommand request, CancellationToken cancellationToken)
    {
        var doctorExists = await _context.Accounts
            .AnyAsync(d => d.Id == request.DoctorId, cancellationToken);

        if (!doctorExists)
        {
            return false;
        }

        var existingSchedule = await _context.Schedules
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

        _context.Schedules.Add(newSchedule);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
