using Mock3.Repositories;

namespace Mock3.Persistence
{
    public interface IUnitOfWork
    {
        IExamRepository Exams { get; }
        IUserExamRepository UserExams { get; }
        IVoucherRepository Vouchers { get; }
        IInvoiceRepository Invoices { get; }
        IUrgentScoreRepository UrgentScores { get; }
        void Complete();
    }
}