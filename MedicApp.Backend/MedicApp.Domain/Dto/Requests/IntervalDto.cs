namespace MedicApp.Domain.Dto.Requests;

public record IntervalDto(
    TimeOnly StartTime,
    TimeOnly EndTime
);