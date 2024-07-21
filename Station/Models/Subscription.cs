using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Station.Models
{

    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        public string? Type { get; set; }
        public int Price { get; set; }
        

        public Subscriber? Subscriber { get; set; }


    }
}
    
