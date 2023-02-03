using Mock3.Models;

namespace Mock3.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
        }
    }
}