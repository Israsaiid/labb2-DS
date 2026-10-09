using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionLab.Data.Entities;

namespace AuctionLab.Data.Interfaces
{
    public interface IAuctionRepository
    {
        Task<List<Auction>> GetActiveAuctionsAsync();
        Task<Auction?> GetAuctionWithBidsAsync(int id);
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task CreateAuctionAsync(Auction auction);
        Task UpdateAuctionAsync(Auction auction);

        Task<List<Auction>> GetMyActiveBidsAsync(string userId);
        Task<List<Auction>> GetEndedAuctionsWithBidsAsync();

        Task AddBidAsync(Bid bid);
    }
}
