using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Station.Models;
using Station.componnent;

namespace Station.Models
{
    public class Meter
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
     [Display(Name ="رقم العداد")]
        [UniqueAttribute(ErrorMessage = "This meter number already exists.")]

        public int numberMeter { get; set; }
        public int price { get; set; }
        public DateTime dateTime { get; set; }
        public int ItemId { get; set; }
        public Subscriber? Subscriber { get; set; }
        public Item? Item { get; set; }
        public ICollection<Reading>? reading { get; set; }
        public ICollection<Bill> ?Bills { get; set; }

    }
}
