namespace AuctionLab.Web.Models
{
    public class ActiveAuctionVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal StartPrice { get; set; }
        public DateTime EndTime { get; set; }

        public string SellerId { get; set; } = "";
        public string SellerName { get; set; } = "";
    }
}
