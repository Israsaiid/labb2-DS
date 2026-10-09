using AuctionLab.Data.Entities;

namespace AuctionLab.Business.Interfaces
{
    public interface IAuctionService
    {
        Task<List<Auction>> GetActiveAuctionsAsync();
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task CreateAuctionAsync(Auction auction);
        Task<string?> PlaceBidAsync(int auctionId, string bidderId, decimal amount);
        Task UpdateDescriptionAsync(int id, string description);
        Task<List<Auction>> GetMyActiveBidsAsync(string userId);
        Task<List<Auction>> GetWonAuctionsAsync(string userId);

    }
}