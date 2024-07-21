using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Station.Models
{
    [Authorize]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public bool IsUpdated { get; set; }
        [Required(ErrorMessage = "يجب ملى الحقل")]
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "الاسم يجب أن يحتوي على أحرف عربية فقط.")]
        [DisplayName(" اسم النفطة")]

        public string Name { get; set; }
        //[Range(10, 99, ErrorMessage = "price  must be between 10 and 99.")] to price
        [Required(ErrorMessage="يجب ملى الحقل")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهاتف يجب أن يكون 9 أرقام.")]
        [DisplayName("رقم الجوال")]
        public double Number_phone { get; set; }
        [DisplayName("التاريخ")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage="يجب تحديد الفرع")]

        
        public int PointId { get; set; }
        public Point? Point { get; set; }

        public ICollection<Subscriber>? Subscriber { get; set; }
        public ICollection<Meter>? Meter { get; set; }
        public ICollection<Reading>? Readings { get; set; }

    }
}
