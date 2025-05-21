using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutoringWebsite.API.Models
{
    public class Payment
    {
        [Key]
        public int ID { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public int StudentId { get; set; }
        [Required]
        public required string PaymentMethod { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public required string Status { get; set; }
        [Required]
        public required string TransactionId { get; set; }
    }
    public enum PaymentType
    {
        Visa=1,
        MasterCard=2,
        PayPal=3,
    }
}
