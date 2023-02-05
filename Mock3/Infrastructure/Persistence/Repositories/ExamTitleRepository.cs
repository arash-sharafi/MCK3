using System.Collections.Generic;
using System.Linq;
using Mock3.Core.Models;
using Mock3.Core.Repositories;

namespace Mock3.Infrastructure.Persistence.Repositories
{
    public class ExamTitleRepository : IExamTitleRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamTitleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ExamTitle> GetExamTitles()
        {
            return _context.ExamTitles.ToList();
        }

        public ExamTitle GetExamTitleById(int examTitleId)
        {
            return _context.ExamTitles.FirstOrDefault(x => x.Id == examTitleId);
        }

        public void Add(ExamTitle examTitle)
        {
            _context.ExamTitles.Add(examTitle);
        }
    }
}