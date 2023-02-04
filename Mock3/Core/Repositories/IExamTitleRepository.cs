using System.Collections.Generic;
using Mock3.Core.Models;

namespace Mock3.Core.Repositories
{
    public interface IExamTitleRepository
    {
        IEnumerable<ExamTitle> GetExamTitles();
        ExamTitle GetExamTitleById(int examTitleId);
        void Add(ExamTitle examTitle);
    }
}