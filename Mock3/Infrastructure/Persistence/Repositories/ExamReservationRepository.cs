using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Mock3.Core.Models;
using Mock3.Core.Repositories;

namespace Mock3.Infrastructure.Persistence.Repositories
{
    public class ExamReservationRepository : IExamReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ExamReservation GetUserExamByForeignKeys(string userId, int examId, int voucherId)
        {
            return _context.ExamReservations
                .FirstOrDefault(x => x.UserId == userId
                                     && x.ExamId == examId
                                     && x.VoucherId == voucherId);
        }

        public ExamReservation GetUserExamByForeignKeys(string userId, int examId)
        {
            return _context.ExamReservations
                .FirstOrDefault(x => x.UserId == userId
                                     && x.ExamId == examId);
        }

        public ExamReservation GetUserExamByForeignKeys(int voucherId, string userId)
        {
            return _context.ExamReservations
                .FirstOrDefault(
                    x => x.UserId == userId
                    && x.VoucherId == voucherId);
        }

        public ExamReservation GetUserExamByVoucherId(int voucherId)
        {
            return _context.ExamReservations.FirstOrDefault(x => x.VoucherId == voucherId);
        }
        public bool Any()
        {
            return _context.ExamReservations.Any();
        }



        public async Task<IEnumerable<ExamReservation>> GetUserExamsByExamId(int examId, bool withDependencies)
        {
            return withDependencies
                ? await _context.ExamReservations
                    .Where(x => x.ExamId == examId)
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.ExamTitle)
                    .Include(x => x.Voucher).ToListAsync()
                : await _context.ExamReservations
                    .Where(x => x.ExamId == examId).ToListAsync();
        }


        public async Task<IEnumerable<ExamReservation>> GetUserExamsByUserId(string userId, bool withDependencies)
        {
            return withDependencies
                ? await _context.ExamReservations
                    .Where(x => x.UserId == userId)
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.ExamTitle)
                    .Include(x => x.Voucher).ToListAsync()
                : await _context.ExamReservations
                    .Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ExamReservation> GetUserExamById(int userExamId, bool withDependencies)
        {
            return !withDependencies
                ? await _context.ExamReservations.FirstOrDefaultAsync(x => x.Id == userExamId)
                : await _context.ExamReservations
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.Voucher)
                    .Include(x => x.ExamTitle)
                    .FirstOrDefaultAsync(x => x.Id == userExamId);
        }


        public void Add(ExamReservation examReservation)
        {
            _context.ExamReservations.Add(examReservation);
        }

        public void Remove(ExamReservation examReservation)
        {
            _context.ExamReservations.Remove(examReservation);
        }
    }
}