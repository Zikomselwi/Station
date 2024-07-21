using System.ComponentModel.DataAnnotations;
using Station.Models;
using Station.componnent;

namespace Station.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }
        public bool IsUpdate { get; set; }
        [UniqueAttribute(ErrorMessage = "This meter number already exists.")]

        public int SubscriberNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الاسم الكامل")]
        public string? FullName { get; set; }

        public int Phone { get; set; }

        [Required(ErrorMessage = "يرجى إدخال العنوان")]
        public string? Address { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public int MeterId { get; set; }
        public int PointId { get; set; }
        public int SubscriptionId { get; set; }

        public Item? Item { get; set; }
        public Subscription? Subscription { get; set; }
        public Meter? Meter { get; set; }
        public ICollection<Bill>? Bills { get; set; }
    }
}