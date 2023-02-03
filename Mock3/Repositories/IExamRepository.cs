using System.Collections.Generic;
using Mock3.Models;

namespace Mock3.Repositories
{
    public interface IExamRepository
    {
        IEnumerable<Exam> GetExams();
        Exam GetExamById(int examId);
    }
}