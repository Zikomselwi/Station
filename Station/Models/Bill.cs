using Station.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Station.Models
{
    public class Bill
    {


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public int numberbill { get; set; }

        [Required]
        public double CurrentReading { get; set; }

        public double? PreviousReading { get; set; }

        public double? ConsumptionDifference { get; set; }

        [Required]
        public decimal ConsumptionCost { get; set; }

        [Required]
        public DateTime BillDate { get; set; }

        public bool IsPaid { get; set; } = false;

        [MaxLength(50)]
        public string? SubscriptionType { get; set; }

        // Foreign Key to Meter
       

        [ForeignKey("Meter")]

        public int MeterId { get; set; }
        public virtual Meter? Meter { get; set; }
        [ForeignKey("Subscriber")]
        public int subscriberId { get; set; }
        public Subscriber Subscriber { get; set; }

    }
}

