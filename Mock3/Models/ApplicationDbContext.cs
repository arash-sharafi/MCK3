using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Mock3.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<UserExam> UserExams { get; set; }


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