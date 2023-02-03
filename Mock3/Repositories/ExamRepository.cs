using Mock3.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mock3.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Exam> GetExams()
        {
            return _context.Exams.ToList();
        }

        public Exam GetExamById(int examId)
        {
            return _context.Exams
                .Find(examId);
        }
    }
}