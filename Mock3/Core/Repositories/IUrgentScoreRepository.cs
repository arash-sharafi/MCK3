using Mock3.Core.Models;
using System.Collections.Generic;

namespace Mock3.Core.Repositories
{
    public interface IUrgentScoreRepository
    {
        UrgentScore GetUrgentScoreByUserExamId(int userExamId);
        IEnumerable<UrgentScore> GetSubmittedUrgentScores();
        IEnumerable<UrgentScore> GetDoneUrgentScores();
        void Add(UrgentScore urgentScore);
    }
}