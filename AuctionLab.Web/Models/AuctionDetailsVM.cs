using System;
using System.Collections.Generic;

namespace AuctionLab.Web.Models
{
    public class AuctionDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal StartPrice { get; set; }
        public DateTime EndTime { get; set; }

        public decimal HighestBid { get; set; }
        public string HighestBidderName { get; set; } = "";

        public string SellerId { get; set; } = "";
        public string SellerName { get; set; } = "";

        public List<BidVM> Bids { get; set; } = new();
    }
}
