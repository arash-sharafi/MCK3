using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class UrgentScore
    {
        public int Id { get; set; }
        
        [Required]
        public int Status { get; set; }

        [Required]
        [StringLength(10)]
        public string SubmitDate { get; set; }



        [Required] 
        public int InvoiceId { get; set; }
        [Required] 
        public int ExamReservationId { get; set; }
        [Required] 
        public int VoucherId { get; set; }
        [Required] 
        public string UserId { get; set; }

    }
}