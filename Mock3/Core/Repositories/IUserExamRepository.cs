using Mock3.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mock3.Core.Repositories
{
    public interface IUserExamRepository
    {
        UserExam GetUserExamByForeignKeys(string userId, int examId, int voucherId);
        UserExam GetUserExamByForeignKeys(string userId, int examId);
        UserExam GetUserExamByForeignKeys(int voucherId, string userId);
        UserExam GetUserExamByVoucherId(int voucherId);
        bool Any();
        Task<IEnumerable<UserExam>> GetUserExamsByExamId(int examId, bool withDependencies);
        Task<IEnumerable<UserExam>> GetUserExamsByUserId(string userId, bool withDependencies);
        Task<UserExam> GetUserExamById(int userExamId, bool withDependencies);
        void Add(UserExam userExam);
        void Remove(UserExam userExam);
    }
}