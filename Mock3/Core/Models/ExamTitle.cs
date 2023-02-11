using System;
using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class ExamTitle
    {
        private ExamTitle(string title)
        {
            Title = title;
        }

        public int Id { get; private set; }

        [Required]
        [StringLength(100)]
        public string Title { get; private set; }

        public void Update(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        public static ExamTitle Create(string title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            return new ExamTitle(title);
        }

        protected ExamTitle()
        {

        }
    }
}