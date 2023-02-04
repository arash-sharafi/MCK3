using Mock3.Core.Models;
using Mock3.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Mock3.Persistence.Repositories
{
    public class UserExamRepository : IUserExamRepository
    {
        private readonly ApplicationDbContext _context;

        public UserExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserExam GetUserExamByForeignKeys(string userId, int examId, int voucherId)
        {
            return _context.UserExams
                .FirstOrDefault(x => x.UserId == userId
                                     && x.ExamId == examId
                                     && x.VoucherId == voucherId);
        }

        public UserExam GetUserExamByForeignKeys(string userId, int examId)
        {
            return _context.UserExams
                .FirstOrDefault(x => x.UserId == userId
                                     && x.ExamId == examId);
        }

        public UserExam GetUserExamByForeignKeys(int voucherId, string userId)
        {
            return _context.UserExams
                .FirstOrDefault(
                    x => x.UserId == userId
                    && x.VoucherId == voucherId);
        }

        public UserExam GetUserExamByVoucherId(int voucherId)
        {
            return _context.UserExams.FirstOrDefault(x => x.VoucherId == voucherId);
        }
        public bool Any()
        {
            return _context.UserExams.Any();
        }



        public async Task<IEnumerable<UserExam>> GetUserExamsByExamId(int examId, bool withDependencies)
        {
            return withDependencies
                ? await _context.UserExams
                    .Where(x => x.ExamId == examId)
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.ExamTitle)
                    .Include(x => x.Voucher).ToListAsync()
                : await _context.UserExams
                    .Where(x => x.ExamId == examId).ToListAsync();
        }


        public async Task<IEnumerable<UserExam>> GetUserExamsByUserId(string userId, bool withDependencies)
        {
            return withDependencies
                ? await _context.UserExams
                    .Where(x => x.UserId == userId)
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.ExamTitle)
                    .Include(x => x.Voucher).ToListAsync()
                : await _context.UserExams
                    .Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<UserExam> GetUserExamById(int userExamId, bool withDependencies)
        {
            return !withDependencies
                ? await _context.UserExams.FirstOrDefaultAsync(x => x.Id == userExamId)
                : await _context.UserExams
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.Voucher)
                    .Include(x => x.ExamTitle)
                    .FirstOrDefaultAsync(x => x.Id == userExamId);
        }


        public void Add(UserExam userExam)
        {
            _context.UserExams.Add(userExam);
        }

        public void Remove(UserExam userExam)
        {
            _context.UserExams.Remove(userExam);
        }
    }
}