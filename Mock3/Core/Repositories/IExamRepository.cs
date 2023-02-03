using System.Collections.Generic;
using Mock3.Core.Models;

namespace Mock3.Core.Repositories
{
    public interface IExamRepository
    {
        IEnumerable<Exam> GetExams();
        Exam GetExamById(int examId);
    }
}