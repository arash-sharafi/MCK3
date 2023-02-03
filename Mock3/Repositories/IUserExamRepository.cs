using System.Collections.Generic;
using Mock3.Models;

namespace Mock3.Repositories
{
    public interface IUserExamRepository
    {
        UserExam GetUserExamByForeignKeys(string userId, int? examId, int? voucherId);
        UserExam GetUserExamByForeignKeys(string userId, int? examId);
        bool Any();
        IEnumerable<UserExam> GetUserExams(int examId);
        IEnumerable<UserExam> GetUserExamWithDependenciesByUserId(string UserId);
        void Add(UserExam userExam);
    }
}