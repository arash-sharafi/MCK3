using Mock3.Core.Models;

namespace Mock3.Core.Repositories
{
    public interface IUrgentScoreRepository
    {
        UrgentScore GetUrgentScoreByUserExamId(int userExamId);
        void Add(UrgentScore urgentScore);
    }
}