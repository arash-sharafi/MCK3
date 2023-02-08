using Mock3.Core;
using Mock3.Core.Repositories;
using Mock3.Infrastructure.Persistence.Repositories;

namespace Mock3.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IExamRepository Exams { get; private set; }
        public IExamReservationRepository ExamsReservation { get; private set; }
        public IVoucherRepository Vouchers { get; private set; }
        public IInvoiceRepository Invoices { get; private set; }
        public IUrgentScoreRepository UrgentScores { get; private set; }
        public IExamTitleRepository ExamTitles { get; private set; }
        public IUserRepository Users { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Exams = new ExamRepository(context);
            ExamsReservation = new ExamReservationRepository(context);
            Vouchers = new VoucherRepository(context);
            Invoices = new InvoiceRepository(context);
            UrgentScores = new UrgentScoreRepository(context);
            ExamTitles = new ExamTitleRepository(context);
            Users = new UserRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}