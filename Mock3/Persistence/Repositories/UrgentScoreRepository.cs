using Mock3.Core.Models;
using Mock3.Core.Repositories;
using Mock3.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Mock3.Persistence.Repositories
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

        public IEnumerable<UrgentScore> GetSubmittedUrgentScores()
        {
            return _context.UrgentScores
                .Where(x => x.Status == (int)UrgentScoreStatus.Submitted)
                .OrderBy(x => x.SubmitDate)
                .ToList();
        }

        public IEnumerable<UrgentScore> GetDoneUrgentScores()
        {
            return _context.UrgentScores
                .Where(x => x.Status != (int)UrgentScoreStatus.Submitted)
                .OrderBy(x => x.SubmitDate)
                .ToList();
        }

        public void Add(UrgentScore urgentScore)
        {
            _context.UrgentScores.Add(urgentScore);
        }
    }
}