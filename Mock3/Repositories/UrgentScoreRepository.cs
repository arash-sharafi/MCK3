using System.Linq;
using Mock3.Models;

namespace Mock3.Repositories
{
    public class UrgentScoreRepository : IUrgentScoreRepository
    {
        private readonly ApplicationDbContext _context;

        public UrgentScoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public UrgentScore GetUrgentScoreByUserExamId(int userExamId)
        {
            return _context.UrgentScores
                .FirstOrDefault(x => x.UserExamId == userExamId);
        }

        public void Add(UrgentScore urgentScore)
        {
            _context.UrgentScores.Add(urgentScore);
        }
    }
}