namespace MedicApp.Api.Endpoints.Doctor.PatientDetail.PatientDetailWithCommentsAndMedicalHistory;


    public static class PatientHistoryMapper
    {
        public static PatientHistoryDto PatientHistiryToDto(this Infrastructure.Models.Patient? patient)
        {
            if (patient == null)
                return null;

            return new PatientHistoryDto
            {
                Id = patient.Id,
                AccountInfo = new AccountInfoDto
                {
                    Id = patient.Account.Id,
                    FullName = $"{patient.Account.Firstname} {patient.Account.Lastname}",
                    Email = patient.Account.Email,
                    PhoneNumber = patient.Account.Phonenumber,
                    CreatedAt = patient.Account.Createdat,
                    Addresses = patient.Account.Addresses.Select(a => new AddressDto
                    {
                        Id = a.Id,
                        Street = a.Street,
                        Building = a.Building,
                        Appartaments = a.Appartaments,
                        City = a.City,
                        Country = a.Country,
                    }).ToList()
                },
                MedicalRequests = patient.MedicalHelpRequests.Select(mr => new MedicalRequestDto
                {
                    Id = mr.Id,
                    Description = mr.Description,
                    CreatedAt = mr.CreateAt,
                    Status = new StatusDto
                    {
                        Id = mr.Status.Id,
                        Name = mr.Status.Name
                    },
                    Comments = mr.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Text = c.CommentText,
                        CreatedAt = c.CreatedAt,
                        AuthorId = c.AuthorId,
                        AuthorName = c.Author != null ? $"{c.Author.Firstname} {c.Author.Lastname}" : "Unknown",
                        Adequacy = c.Adequacy
                    }).ToList(),
                    ScheduleInfo = mr.ScheduleInterval != null ? new ScheduleInfoDto
                    {
                        StartTime = mr.ScheduleInterval.StartTime,
                        EndTime = mr.ScheduleInterval.EndTime,
                        Date = mr.ScheduleInterval.Schedule?.Date
                    } : null
                }).ToList()
            };
        }
    }

    public class PatientHistoryDto
    {
        public int Id { get; set; }
        public AccountInfoDto AccountInfo { get; set; }
        public List<MedicalRequestDto> MedicalRequests { get; set; } = new List<MedicalRequestDto>();
    }

    public class AccountInfoDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Appartaments { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class MedicalRequestDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public StatusDto Status { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public ScheduleInfoDto? ScheduleInfo { get; set; }
    }

    public class StatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int? Adequacy { get; set; }
    }
    public class ScheduleInfoDto
    {
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public DateOnly? Date { get; set; }
    }
