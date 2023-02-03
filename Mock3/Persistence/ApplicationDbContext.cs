using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mock3.Core.Models;

namespace Mock3.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<UserExam> UserExams { get; set; }
        public DbSet<ExamTitle> ExamTitles { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PurchaseType> PurchaseTypes { get; set; }
        public DbSet<UrgentScore> UrgentScores { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}