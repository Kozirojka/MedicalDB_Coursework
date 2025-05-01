using MedicApp.Domain.Dto.Requests;
using MedicApp.Infrastructure.Models;

namespace MedicApp.Api.Endpoints.Doctor.Schedule.GetIntervas;

public static class ScheduleMapper
{
    public static List<ScheduleDto> MapSchedules(List<Infrastructure.Models.Schedule> schedules)
    {
        return schedules.Select(s => new ScheduleDto
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            Date = s.Date,
            Intervals = s.ScheduleIntervals.Select(si => new ScheduleIntervalDto
            {
                Id = si.Id,
                StartTime = si.StartTime,
                EndTime = si.EndTime,
                IsBooked = si.MedicalHelpRequests.Any()
            }).ToList()
        }).ToList();
    }
}

public class ScheduleDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public List<ScheduleIntervalDto> Intervals { get; set; } = new List<ScheduleIntervalDto>();
}

public class ScheduleIntervalDto
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsBooked { get; set; } // Визначає, чи зайнятий інтервал
}