namespace AuctionLab.Web.Models
{
    public class CreateAuctionVM
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal StartPrice { get; set; }
        public DateTime EndTime { get; set; }
    }
}
