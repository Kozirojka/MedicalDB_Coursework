using MedicApp.Infrastructure.Models;

namespace MedicApp.Api.Endpoints.Patient.GetInProgressRequests;

public static class RequestMapper
{
    public static List<MedicalHelpDto> MapToDto(this List<MedicalHelpRequest> requests)
    {
        if (requests == null)
            return new List<MedicalHelpDto>();
            
        return requests.Select(request => new MedicalHelpDto
        {
            Id = request.Id,
            Description = request.Description,
            CreatedAt = request.CreateAt,
            StatusName = request.Status?.Name,
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            DoctorName = request.Doctor.Account.Firstname + " " + request.Doctor.Account.Lastname,
            ScheduleInfo = request.ScheduleInterval != null ? new ScheduleInfoDto
            {
                IntervalId = request.ScheduleInterval.Id,
                StartTime = request.ScheduleInterval.StartTime,
                EndTime = request.ScheduleInterval.EndTime,
                Date = request.ScheduleInterval.Schedule.Date,
            } : null
        }).ToList();
    }
}

public class MedicalHelpDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string StatusName { get; set; }
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public ScheduleInfoDto? ScheduleInfo { get; set; }
}

public class ScheduleInfoDto
{
    public int IntervalId { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly Date { get; set; }
}