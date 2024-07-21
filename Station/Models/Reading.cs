using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Station.Models
{
    public class Reading
    {
        [Key]
        public int Id { get; set; }
        public float CurrentRead { get; set; }
        public DateTime dateTime { get; set; }

        public int MeterId { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        public Meter? Meter { get; set; }

    }
}
