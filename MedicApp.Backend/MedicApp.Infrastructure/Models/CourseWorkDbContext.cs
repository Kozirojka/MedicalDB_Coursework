using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Infrastructure.Models;

public partial class CourseWorkDbContext : DbContext
{
    public CourseWorkDbContext()
    {
    }

    public CourseWorkDbContext(DbContextOptions<CourseWorkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<HelpRequestStatus> HelpRequestStatuses { get; set; }

    public virtual DbSet<MedicalHelpRequest> MedicalHelpRequests { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<RequestMedicine> RequestMedicines { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleInterval> ScheduleIntervals { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CourseWork_2;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Account_pkey");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Phonenumber, "account_phonenumber_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_role");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("address_pkey");

            entity.ToTable("address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Appartaments)
                .HasMaxLength(50)
                .HasColumnName("appartaments");
            entity.Property(e => e.Building)
                .HasMaxLength(50)
                .HasColumnName("building");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("doctor_pkey");

            entity.ToTable("doctor");

            entity.HasIndex(e => e.AccountId, "uq_account_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");

            entity.HasOne(d => d.Account).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account");

            entity.HasMany(d => d.Specializations).WithMany(p => p.Doctors)
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorSpecialization",
                    r => r.HasOne<Specialization>().WithMany()
                        .HasForeignKey("SpecializationId")
                        .HasConstraintName("fk_specialization"),
                    l => l.HasOne<Doctor>().WithMany()
                        .HasForeignKey("DoctorId")
                        .HasConstraintName("fk_doctor"),
                    j =>
                    {
                        j.HasKey("DoctorId", "SpecializationId").HasName("pk_doctor_specialization");
                        j.ToTable("doctor_specialization");
                        j.IndexerProperty<int>("DoctorId").HasColumnName("doctor_id");
                        j.IndexerProperty<int>("SpecializationId").HasColumnName("specialization_id");
                    });
        });

        modelBuilder.Entity<HelpRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("help_request_status_pkey");

            entity.ToTable("help_request_status");

            entity.HasIndex(e => e.Name, "help_request_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MedicalHelpRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medical_help_request_pkey");

            entity.ToTable("medical_help_request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicalHelpRequests)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalHelpRequests)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_patient");

            entity.HasOne(d => d.Status).WithMany(p => p.MedicalHelpRequests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_status");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicines_pkey");

            entity.ToTable("medicines");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvailableQuantity)
                .HasPrecision(10, 2)
                .HasColumnName("available_quantity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("patient_pkey");

            entity.ToTable("patient");

            entity.HasIndex(e => e.AccountId, "uq_patient_account").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");

            entity.HasOne(d => d.Account).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_patient_account");
        });

        modelBuilder.Entity<RequestMedicine>(entity =>
        {
            entity.HasKey(e => e.RequestMedicineId).HasName("request_medicines_pkey");

            entity.ToTable("request_medicines");

            entity.Property(e => e.RequestMedicineId).HasColumnName("request_medicine_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.MedicineId).HasColumnName("medicine_id");
            entity.Property(e => e.QuantityProvided)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("quantity_provided");
            entity.Property(e => e.QuantityRequired)
                .HasPrecision(10, 2)
                .HasColumnName("quantity_required");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Medicine).WithMany(p => p.RequestMedicines)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_medicines_medicine_id_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestMedicines)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_medicines_request_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "Role_Name_key").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("schedule_pkey");

            entity.ToTable("schedule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("fk_doctor");
        });

        modelBuilder.Entity<ScheduleInterval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("schedule_interval_pkey");

            entity.ToTable("schedule_interval");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleIntervals)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("fk_schedule");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specialization_pkey");

            entity.ToTable("specialization");

            entity.HasIndex(e => e.Name, "specialization_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
