using Shivakala.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shivakala.Core.Entities;
using System;

namespace Shivakala.Infrastructure.Data
{
    public sealed class ShivakalaDbContext : DbContext
    {
        private static readonly ValueConverter<DateTime, DateTime> UtcDateTimeConverter = new(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        public ShivakalaDbContext(DbContextOptions<ShivakalaDbContext> options) : base(options)
        {
        }

        // Core Entities (from Shivakala.Core.Entities)
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enquiry> Enquiries => Set<Enquiry>();
        public DbSet<Notice> Notices => Set<Notice>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<StudyMaterial> StudyMaterials => Set<StudyMaterial>();
        public DbSet<GalleryItem> GalleryItems => Set<GalleryItem>();
        public DbSet<Testimonial> Testimonials => Set<Testimonial>();

        // Settings
        public DbSet<HomePageSectionSettings> HomePageSectionSettings => Set<HomePageSectionSettings>();
        public DbSet<AboutPageSectionSettings> AboutPageSectionSettings => Set<AboutPageSectionSettings>();

        // User Management
        public DbSet<AppUser> AppUsers => Set<AppUser>();

        // Batch Management
        public DbSet<Batch> Batches => Set<Batch>();
        public DbSet<BatchSubject> BatchSubjects => Set<BatchSubject>();
        public DbSet<StudentBatch> StudentBatches => Set<StudentBatch>();

        // Attendance
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<TeacherAttendance> TeacherAttendances => Set<TeacherAttendance>();

        // Fees
        public DbSet<FeeStructure> FeeStructures => Set<FeeStructure>();
        public DbSet<FeePayment> FeePayments => Set<FeePayment>();

        // Exams (from Entities)
        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<ExamResult> ExamResults => Set<ExamResult>();

        // Homework
        public DbSet<Homework> Homeworks => Set<Homework>();
        public DbSet<HomeworkSubmission> HomeworkSubmissions => Set<HomeworkSubmission>();

        // Timetable
        public DbSet<TimetableSlot> TimetableSlots => Set<TimetableSlot>();

        // Notifications
        public DbSet<Notification> Notifications => Set<Notification>();

        // Audit
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // Syllabus
        public DbSet<SyllabusItem> SyllabusItems => Set<SyllabusItem>();

        // Paper Generator - New Features (from Shivakala.Core.Models)
        public DbSet<ExamPaper> ExamPapers => Set<ExamPaper>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<StudentTestAttempt> StudentTestAttempts => Set<StudentTestAttempt>();
        public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure all DateTime properties to use UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(UtcDateTimeConverter);
                    }
                }
            }
        }
    }
}
