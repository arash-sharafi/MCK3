using Mock3.Models;
using Mock3.Repositories;

namespace Mock3.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository Exams { get; private set; }
        public UserExamRepository UserExams { get; private set; }
        public VoucherRepository Vouchers { get; private set; }
        public InvoiceRepository Invoices { get; private set; }
        public UrgentScoreRepository UrgentScores { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Exams = new ExamRepository(context);
            UserExams = new UserExamRepository(context);
            Vouchers = new VoucherRepository(context);
            Invoices = new InvoiceRepository(context);
            UrgentScores = new UrgentScoreRepository(context);

        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}