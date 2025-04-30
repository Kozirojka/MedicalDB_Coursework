namespace MedicApp.Domain.Dto.Requests;

public record IntervalDto(
    DateTime StartTime,
    DateTime EndTime,
    int? MedicHelp
);  