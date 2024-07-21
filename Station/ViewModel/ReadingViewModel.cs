using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Station.ViewModel
{
    public class ReadingViewModel
    {

        public int Id { get; set; } 

 
        [Required(ErrorMessage = "يرجى إدخال  اختيار العداد")]
        public int MeterId { get; set; }
        [Required(ErrorMessage = "يرجى إدخال  التاريخ")]
        public DateTime? Created { get; set; }
        [Required(ErrorMessage = "يرجى إدخال  القراءة")]
        public float CurrentRead { get; set; }

        public int Phone { get; set; }

        public int PointId { get; set; }

        public IEnumerable<SelectListItem>? Meters { get; set; }
        public IEnumerable<SelectListItem>? Items { get; set; }

    }
}


