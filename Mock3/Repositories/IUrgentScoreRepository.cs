using Mock3.Models;

namespace Mock3.Repositories
{
    public interface IUrgentScoreRepository
    {
        UrgentScore GetUrgentScoreByUserExamId(int userExamId);
        void Add(UrgentScore urgentScore);
    }
}