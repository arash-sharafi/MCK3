using Mock3.Core;
using Mock3.Core.Models;
using Mock3.Core.Repositories;
using Mock3.Persistence.Repositories;

namespace Mock3.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IExamRepository Exams { get; private set; }
        public IUserExamRepository UserExams { get; private set; }
        public IVoucherRepository Vouchers { get; private set; }
        public IInvoiceRepository Invoices { get; private set; }
        public IUrgentScoreRepository UrgentScores { get; private set; }

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