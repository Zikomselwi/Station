using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Station.ViewModel
{
    public class SubscriberViewModel
    {
        public int Id { get; set; }
        public bool IsUpdate { get; set; }
        public int SubscriberNumber { get; set; }
        public DateTime dataTime  { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الاسم الكامل")]
        public string? FullName { get; set; }

        public int Phone { get; set; }

        [Required(ErrorMessage = "يرجى إدخال العنوان")]
        public string? Address { get; set; }

        public int MeterId { get; set; }
        public int PointId { get; set; }
        public int SubscriptionId { get; set; }

        public IEnumerable<SelectListItem> ?Items { get; set; }
        public IEnumerable<SelectListItem> ?Meters { get; set; }
        public IEnumerable<SelectListItem>? Subscriptions { get; set; }
    }

}
