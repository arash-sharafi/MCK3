using Mock3.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mock3.Core.Repositories
{
    public interface IExamReservationRepository
    {
        ExamReservation GetUserExamByForeignKeys(string userId, int examId, int voucherId);
        ExamReservation GetUserExamByForeignKeys(string userId, int examId);
        ExamReservation GetUserExamByForeignKeys(int voucherId, string userId);
        ExamReservation GetUserExamByVoucherId(int voucherId);
        bool Any();
        Task<IEnumerable<ExamReservation>> GetUserExamsByExamId(int examId, bool withDependencies);
        Task<IEnumerable<ExamReservation>> GetUserExamsByUserId(string userId, bool withDependencies);
        Task<ExamReservation> GetUserExamById(int userExamId, bool withDependencies);
        void Add(ExamReservation examReservation);
        void Remove(ExamReservation examReservation);
    }
}