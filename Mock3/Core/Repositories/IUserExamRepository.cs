using Mock3.Core.Models;
using System.Collections.Generic;

namespace Mock3.Core.Repositories
{
    public interface IUserExamRepository
    {
        UserExam GetUserExamByForeignKeys(string userId, int examId, int voucherId);
        UserExam GetUserExamByForeignKeys(string userId, int examId);
        UserExam GetUserExamByForeignKeys(int voucherId, string userId);
        UserExam GetUserExamByVoucherId(int voucherId);
        bool Any();
        IEnumerable<UserExam> GetUserExams(int examId);
        IEnumerable<UserExam> GetUserExamWithDependenciesByUserId(string userId);
        UserExam GetUserExamById(int userExamId, bool withDependencies);
        void Add(UserExam userExam);
        void Remove(UserExam userExam);
    }
}