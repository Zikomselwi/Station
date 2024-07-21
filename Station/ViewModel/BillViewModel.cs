using System;
using System.Collections.Generic;
using Station.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Station.ViewModel
{
    public class BillViewModel
    {

        public List<Meter>? Meters { get; set; }
        public List<Subscriber>? Subscribers { get; set; }
        public List<Reading>? Readings { get; set; }
        public List<Subscription>? Subscriptions { get; set; }
        public List<Bill>? Bills { get; set; }
        public List<Item>? Items { get; set; }
        public int PointId { get; set; }
        public int numberbill { get; set; }
        public int Id { get; set; }
        public int MeterId { get; set; }

        // New properties
        public int subscriberId { get; set; }
        public double? CurrentReading { get; set; }
        public double? PreviousReading { get; set; }
        public double? ConsumptionDifference { get; set; }
        public decimal? ConsumptionCost { get; set; }
        public bool IsPaid { get; set; }

        public string? SubscriptionType { get; set; }
        public string? SubscriberName{ get; set; }
    }
}


