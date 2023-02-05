using System.Collections.Generic;
using System.Linq;
using Mock3.Core.Models;
using Mock3.Core.Repositories;

namespace Mock3.Infrastructure.Persistence.Repositories
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

        public void Add(Exam exam)
        {
            _context.Exams.Add(exam);
        }

        public void Remove(Exam exam)
        {
            _context.Exams.Remove(exam);
        }
    }
}