using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Station.Models;

namespace Station.Models
{

    public class Point
    {
        [Key]
        public int Id { get; set; }
        //public bool IsUpdated { get; set; }
       [Required(ErrorMessage = "يجب ملى الحقل")]
        [DisplayName(" اسم الفرع")]
        public string? NamePoint { get; set; }

        //[Range(10, 99, ErrorMessage = "price  must be between 10 and 99.")] to price
        public DateTime Date { get; set; } 
        public ICollection<Item> ?Items { get; set; }

    }
}

