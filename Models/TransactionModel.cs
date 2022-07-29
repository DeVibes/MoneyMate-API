using System.ComponentModel.DataAnnotations;

namespace AccountyMinAPI.Models
{
    public record TransactionModel    
    {
        [Key]
        public int Id { get; init; }

        public string? Description { get; init; }

        [Required]
        public string Store { get; init; } = "Not defined";

        [Required]
        public double Price { get; init; } = 0;

        [Required]
        public DateTime Date { get; init; } = DateTime.UtcNow;

        [Required]
        public int PaymentTypeId { get; init; } 

        [Required]
        public int CategoryId { get; init; }
        
        [Required]
        public bool Seen { get; init; } = false;
    }
}