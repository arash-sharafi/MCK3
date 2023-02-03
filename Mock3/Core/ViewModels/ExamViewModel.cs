using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.ViewModels
{
    public class ExamViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string StartDate { get; set; }

        [StringLength(300)] 
        public string Description { get; set; }

        public int Capacity { get; set; }
        
        public int RemainingCapacity { get; set; }

        public bool IsOpen { get; set; }

        public bool IsUserRegisteredBefore { get; set; }

        public string RegisterStatus { get; set; }
    }
}