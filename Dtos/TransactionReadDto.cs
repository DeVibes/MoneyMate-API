using System.ComponentModel.DataAnnotations;

namespace AccountyMinAPI.Dtos
{
    public record TransactionReadDto    
    {
        [Key]
        public int Id { get; init; }

        public string? Description { get; init; }

        [Required]
        public string Store { get; init; } = "Not defined";

        [Required]
        public double Price { get; init; } = 0;

        [Required]
        public DateTime Date { get; init; } = DateTime.Now;

        [Required]
        public int PaymentTypeId { get; init; } 

        [Required]
        public int CategoryId { get; init; }
        
        [Required]
        public bool IsSeen { get; init; }
    }
}