using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UsersApp.Models;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        public decimal? Value { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(30, ErrorMessage = "Description cannot exceed 30 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        public string? Currency { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [RequiredIf(nameof(ShouldValidateDueDate), true, ErrorMessage = "Due date is required if payment is not completed.")]
        public DateTime? PaymentDueDate { get; set; }

        [RequiredIf(nameof(ShouldValidatePaymentDate), true, ErrorMessage = "Payment date is required if payment is not planned.")]
        public DateTime? PaymentDate { get; set; }

        [NotMapped]
        public bool ShouldValidateDueDate => Status != "Completed" && Status != "Delayed";

        [NotMapped]
        public bool ShouldValidatePaymentDate => Status != "Planned";

        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }

        public byte[]? AttachmentData { get; set; }

        public string? AttachmentFileName { get; set; }

        public static List<string> Categories => new()
        {
            "Food", "Transport", "Entertainment", "Bills", "Shopping", "Health", "Education", "Other"
        };
    }
}
