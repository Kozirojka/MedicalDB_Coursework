using Bogus;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.FakeDate;

public class FakeDate
{
    private readonly CourseWork2Context _dbContext;
    private readonly Random _random = new Random();
    private readonly IPasswordService _service;
    

    public FakeDate(CourseWork2Context dbContext, IPasswordService service)
    {
        _dbContext = dbContext;
        _service = service;
        Randomizer.Seed = new Random(_seedValue);
    }

    private readonly int _seedValue = 123456;

    private const int AccountCount = 100;
    private const int AdminCount = 5;
    private const int DoctorCount = 20;
    private const int PatientCount = 75;
    private const int AddressCount = 100;
    private const int HelpRequest = 75;

    private readonly string[] streets =
    [
        "вулиця Академіка Кравчука",
        "вулиця Академіка Богомольця",
        "вулиця Нечуя-Левицького",
        "вулиця Генерала Чупринки",
        "вулиця Генерала Тарнавського",
        "вулиця Чернігівська",
        "вулиця Просвіти",
        "вулиця Мирослава Скорика",
        "вулиця Юрія Руфа",
        "вулиця Черемшини",
        "вулиця Анатолія Вахнянина",
        "вулиця Погулянка",
        "вулиця Липова Алея",
        "вулиця Дністерська",
        "вулиця Академія Ігоря Юхновського",
        "вулиця Княгині Ольги",
        "вулиця Наукова",
        "вулиця Івана Пулюя",
        "вулиця Трускавецька",
        "вулиця Личаківська",
        "вулиця Городоцька",
        "вулиця Шевченка",
        "вулиця Зеленая",
        "вулиця Сахарова",
        "вулиця Пасічна",
        "вулиця Стрийська",
        "вулиця Замарстинівська",
        "вулиця Богдана Хмельницького",
        "вулиця Під Дубом",
        "вулиця Коперника",
        "вулиця Володимира Великого",
        "вулиця Панча",
        "вулиця Мазепи",
        "вулиця Варшавська",
        "вулиця Промислова",
        "вулиця Липинського",
        "вулиця Січових Стрільців",
        "вулиця Франка",
        "вулиця Пекарська",
        "вулиця Кульпарківська",
        "вулиця Шота Руставелі",
        "вулиця Митрополита Андрея",
        "вулиця Чорновола",
        "вулиця Антоновича"
    ];

    private readonly string[] specialization =
    [
        "Traumatologist",
        "Psychotherapist",
        "Obstetrician",
        "Therapist"
    ];

    public bool GenerateTestData()
    {
        CleanDataBase();
        GenerateNonPersonalDate();

        //згенерувати аакаунти 
        GenerateAccountsWithAddresses();

        //згенерувати адмінів
        GenerateAdmins();

        //згенерувати докторів
        GenerateDoctor();
        
        //згенерувати пацієнтів
        GeneratePatients();

        


        return true;
    }

    public void GenerateNonPersonalDate()
    {
        GenerateRoles();
        GenerateEducation();
        GenerateSpecialization();
        GenerateStatusesOfVisitRequest();
    }

    public void CleanDataBase()
    {
        using (var context = new CourseWork2Context())
        {
            var sql = @"
    DO $$ DECLARE
        r RECORD;
        seq RECORD;
    BEGIN

        FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP
            EXECUTE 'TRUNCATE TABLE public.' || quote_ident(r.tablename) || ' CASCADE';
        END LOOP;

        FOR seq IN (
            SELECT sequence_name
            FROM information_schema.sequences
            WHERE sequence_schema = 'public'
        ) LOOP
            EXECUTE 'ALTER SEQUENCE public.' || quote_ident(seq.sequence_name) || ' RESTART WITH 1';
        END LOOP;
    END $$;
    ";

            context.Database.ExecuteSqlRaw(sql);
        }
    }

    private void GenerateEducation()
    {
        var education = new List<Education>
        {
            new () { Name = "Kyiv Medical University" },
            new () { Name = "Bogomolets National Medical University" },
            new () { Name = "Lviv National Medical University" },
            new () { Name = "Kharkiv National Medical University" },
            new () { Name = "Danylo Halytsky Lviv National Medical University" },
            new () { Name = "Vinnytsia National Medical University" },
            new () { Name = "Zaporizhzhia State Medical University" },
            new () { Name = "Odessa National Medical University" },
            new () { Name = "Bukovinian State Medical University" },
            new () { Name = "Ivano-Frankivsk National Medical University" }
        };
        
        _dbContext.Educations.AddRange(education);
         _dbContext.SaveChanges();
        
    }

    private  void GenerateSpecialization()
    {
        var specialization = new List<Specialization>
        {
            new Specialization() { Name = "Traumatologist" },
            new Specialization() { Name = "Psychotherapist" },
            new Specialization() { Name = "Obstetrician" },
            new Specialization() { Name = "Therapist" },
        };

        _dbContext.Specializations.AddRange(specialization);
        _dbContext.SaveChanges();
    }

    private void GenerateStatusesOfVisitRequest()
    {
        var status_of_visit_request = new List<HelpRequestStatus>
        {
            new HelpRequestStatus { Name = "LookingForAssign" },
            new HelpRequestStatus { Name = "AssignedToDoctor" },
            new HelpRequestStatus { Name = "DoctorRejected" },
            new HelpRequestStatus { Name = "InProgress" },
            new HelpRequestStatus { Name = "Completed" },
            new HelpRequestStatus { Name = "CancelledByPatient" },
        };

        _dbContext.HelpRequestStatuses.AddRange(status_of_visit_request);
        _dbContext.SaveChanges();
    }

    private void GenerateRoles()
    {
        var roles = new List<Role>
        {
            new Role { Id = 1, Name = "Patient" },
            new Role { Id = 2, Name = "Doctor" },
            new Role { Id = 3, Name = "Admin" }
        };

        _dbContext.Roles.AddRange(roles);
        _dbContext.SaveChanges();
    }

    private void GenerateAccountsWithAddresses()
    {
        var accountFaker = new Faker<Account>()
            .RuleFor(a => a.Lastname, f => f.Name.LastName())
            .RuleFor(a => a.Firstname, f => f.Name.FirstName())
            .RuleFor(a => a.Phonenumber, f => f.Phone.PhoneNumber("+380#########"))
            .RuleFor(a => a.Createdat, f => f.Date.Past(2))
            .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.Firstname, a.Lastname))
            .RuleFor(a => a.RoleId, f => f.Random.Int(1, 3))
            .RuleFor(a => a.PasswordHash, f => _service.HashPassword("User123A!"));
            
             var accounts = accountFaker.Generate(AccountCount);
            
             for (int i = 0; i < AdminCount; i++)
                    accounts[i].RoleId = 3;
            
             for (int i = AdminCount; i < AdminCount + DoctorCount; i++)
                    accounts[i].RoleId = 2;
            
             for (int i = AdminCount + DoctorCount; i < AccountCount; i++)
                    accounts[i].RoleId = 1;
             
             _dbContext.Accounts.AddRange(accounts);
                 _dbContext.SaveChanges();
                 
                 var addressFaker = new Faker<Address>()
                     .RuleFor(a => a.Street, f => f.PickRandom(streets))
                     .RuleFor(a => a.Building, f => f.Random.Int(1, 50).ToString())
                     .RuleFor(a => a.Appartaments, f => _random.Next(1, 200).ToString())
                     .RuleFor(a => a.Country, f => "Україна")
                     .RuleFor(a => a.City, f => "Львів")
                     .RuleFor(a => a.Region, f => "Львівська область")
                     .RuleFor(a => a.Longitude, f => f.Random.Double(22.0, 40.0))
                     .RuleFor(a => a.Latitude, f => f.Random.Double(44.0, 52.0));
                 
                 var addresses = accounts.Select(acc => {
                     var address = addressFaker.Generate();
                     address.AccountId = acc.Id;
                     return address;
                 }).ToList();
                 
                 _dbContext.Addresses.AddRange(addresses);
                 _dbContext.SaveChanges();
    }
    
    
    
    private void GenerateAccounts()
    {
        var accountFaker = new Faker<Account>()
            .RuleFor(a => a.Lastname, f => f.Name.LastName())
            .RuleFor(a => a.Firstname, f => f.Name.FirstName())
            .RuleFor(a => a.Phonenumber, f => f.Phone.PhoneNumber("+380#########"))
            .RuleFor(a => a.Createdat, f => f.Date.Past(2))
            .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.Firstname, a.Lastname))
            .RuleFor(a => a.RoleId, f => f.Random.Int(1, 3))
            .RuleFor(a => a.PasswordHash, f => _service.HashPassword("User123A!"));

        var accounts = accountFaker.Generate(AccountCount);

        for (int i = 0; i < AdminCount; i++)
            accounts[i].RoleId = 3;

        for (int i = AdminCount; i < AdminCount + DoctorCount; i++)
            accounts[i].RoleId = 2;

        for (int i = AdminCount + DoctorCount; i < AccountCount; i++)
            accounts[i].RoleId = 1;

        _dbContext.Accounts.AddRange(accounts);
        _dbContext.SaveChanges();
    }

    private void GenerateAdmins()
    {
        var adminAccounts = _dbContext.Accounts.Where(a => a.RoleId == 3).Take(AdminCount).ToList();

        var admins = adminAccounts.Select(a => new Admin
        {
            Accountid = a.Id
        }).ToList();

        _dbContext.Admins.AddRange(admins);
        _dbContext.SaveChanges();
    }

        private void GeneratePatients()
        {
            var patientAccounts = _dbContext.Accounts
                .Where(a => a.RoleId == 1)
                .Take(PatientCount)
                .ToList();

            var faker = new Faker();

            var doctors = _dbContext.Doctors.ToList();
            var random = new Random();

            var patients = patientAccounts.Select(a =>
            {
                var patient = new Patient
                {
                    AccountId = a.Id,
                    MedicalHelpRequests = faker.Random.Bool(0.3f)
                        ? new List<MedicalHelpRequest>
                        {
                            new()
                            {
                                Description = faker.Lorem.Sentence(),
                                CreateAt = faker.Date.Recent(7),
                                StatusId = 1,
                                DoctorId = doctors.Any() ? doctors[random.Next(doctors.Count)].Id : null
                            }
                        }
                        : new List<MedicalHelpRequest>()
                };

                return patient;
            }).ToList();

            _dbContext.Patients.AddRange(patients);
            _dbContext.SaveChanges();
        }

    private void GenerateDoctor()
    {
        var doctorAccounts = _dbContext.Accounts
            .Where(a => a.RoleId == 2)
            .Take(PatientCount)
            .ToList();

        var specializations = _dbContext.Specializations.ToList();
        var random = new Random();

        var doctors = doctorAccounts.Select(a =>
        {
            int count = random.Next(1, 3);

            var randomSpecializations = specializations
                .OrderBy(s => random.Next())
                .Take(count)
                .ToList();

            return new Doctor
            {
                AccountId = a.Id,
                Specializations = randomSpecializations
            };
        }).ToList();

        _dbContext.Doctors.AddRange(doctors);
        _dbContext.SaveChanges();
    }

    private void GenerateAddresses()
    {
        var addressFaker = new Faker<Address>()
            .RuleFor(a => a.Street, f => f.PickRandom(streets))
            .RuleFor(a => a.Building, f => f.Random.Int(1, 50).ToString())
            .RuleFor(a => a.Appartaments, f => _random.Next(1, 200).ToString())
            .RuleFor(a => a.Country, f => "Україна")
            .RuleFor(a => a.City, f => f.PickRandom(
                new[] { "Львів" }))
            .RuleFor(a => a.Region, (f, a) => "Львівська область")
            .RuleFor(a => a.Longitude, f => f.Random.Double(22.0, 40.0))
            .RuleFor(a => a.Latitude, f => f.Random.Double(44.0, 52.0))
            .RuleFor(a => a.AccountId, f => f.PickRandom(_dbContext.Accounts.Select(a => a.Id).ToList()));

        var addresses = addressFaker.Generate(AddressCount);
        _dbContext.Addresses.AddRange(addresses);
        _dbContext.SaveChanges();
    }
}