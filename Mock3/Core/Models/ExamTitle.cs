using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class ExamTitle
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

    }
}