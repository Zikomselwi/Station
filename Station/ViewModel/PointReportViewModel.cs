using System.Collections.Generic;

namespace Station.ViewModel
{
    public class PointReportViewModel
    {
        public int PointId { get; set; } // Add this line

        public string PointName { get; set; }
        public string branchName { get; set; }

        public decimal TotalAmountCollected { get; set; }
        public int TotalSubscribers { get; set; }
        public int TotalBillsPaid { get; set; }
        public int TotalBillsUnpaid { get; set; }
        public int TotalMeters { get; set; }
        public int InactiveSubscribers { get; set; }
    }

  
}

