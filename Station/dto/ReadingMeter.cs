using Microsoft.AspNetCore.Mvc.Rendering;

namespace Station.dto
{
    public class ReadingMeter
    {
        public float ?readcurrent { get; set; }
        public int ?meterId{ get; set; }
        public int pointId { get; set; }
       

    }
}
