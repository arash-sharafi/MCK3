using Mock3.Core.Models;
using Mock3.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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



        public IEnumerable<UserExam> GetUserExams(int examId)
        {
            return _context.UserExams.Where(x => x.ExamId == examId).ToList();
        }

        public IEnumerable<UserExam> GetUserExamWithDependenciesByUserId(string userId)
        {
            return _context.UserExams
                .Where(x => x.UserId.Equals(userId))
                .Include(x => x.User)
                .Include(x => x.Exam)
                .Include(x => x.Voucher).ToList();
        }

        public UserExam GetUserExamById(int userExamId, bool withDependencies)
        {
            return !withDependencies
                ? _context.UserExams.FirstOrDefault(x => x.Id == userExamId)
                : _context.UserExams
                    .Include(x => x.User)
                    .Include(x => x.Exam)
                    .Include(x => x.Voucher)
                    .FirstOrDefault(x => x.Id == userExamId);
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