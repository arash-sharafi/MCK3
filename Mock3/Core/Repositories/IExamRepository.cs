using Mock3.Core.Models;
using System.Collections.Generic;

namespace Mock3.Core.Repositories
{
    public interface IExamRepository
    {
        IEnumerable<Exam> GetExams();
        Exam GetExamById(int examId);
        void Add(Exam exam);
        void Remove(Exam exam);
    }
}