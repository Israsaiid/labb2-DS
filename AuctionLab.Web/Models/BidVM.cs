namespace AuctionLab.Web.Models
{
    public class BidVM
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }

        public string BidderId { get; set; } = "";
        public string BidderName { get; set; } = "";
    }
}

