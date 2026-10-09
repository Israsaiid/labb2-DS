namespace AuctionLab.Data.Entities
{
    public class Bid
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string BidderId { get; set; } = "";

        public int AuctionId { get; set; }
        public Auction Auction { get; set; } = null!;
    }
}
