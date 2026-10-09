namespace AuctionLab.Data.Entities
{
    public class Auction
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string SellerId { get; set; } = "";
        public decimal StartPrice { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
