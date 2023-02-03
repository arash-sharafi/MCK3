using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Mock3.Core.Models;
using Mock3.Core.Repositories;

namespace Mock3.Persistence.Repositories
{
    public class UserExamRepository : IUserExamRepository
    {
        private readonly ApplicationDbContext _context;

        public UserExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserExam GetUserExamByForeignKeys(string userId, int? examId, int? voucherId)
        {
            return _context.UserExams
                .FirstOrDefault(x => x.UserId == userId
                                     && x.ExamId == examId
                                     && x.VoucherId == voucherId);
        }

        public UserExam GetUserExamByForeignKeys(string userId, int? examId)
        {
            return _context.UserExams
                .FirstOrDefault(x => x.UserId == userId
                                     && x.ExamId == examId);
        }

        public bool Any()
        {
            return _context.UserExams.Any();
        }



        public IEnumerable<UserExam> GetUserExams(int examId)
        {
            return _context.UserExams.Where(x => x.ExamId == examId).ToList();
        }

        public IEnumerable<UserExam> GetUserExamWithDependenciesByUserId(string UserId)
        {
            return _context.UserExams
                .Where(x => x.UserId.Equals(UserId))
                .Include(x => x.Exam)
                .Include(x => x.Voucher).ToList();
        }


        public void Add(UserExam userExam)
        {
            _context.UserExams.Add(userExam);
        }
    }
}