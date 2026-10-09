//using AuctionLab.Data;
using AuctionLab.Data.Entities;
using AuctionLab.Business.Interfaces;
using AuctionLab.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionLab.Business.Services
{
    public class AuctionService : IAuctionService
    {

        private readonly IAuctionRepository _repository;
        public AuctionService(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Auction>> GetActiveAuctionsAsync()
        {
            return await _repository.GetActiveAuctionsAsync();
        }

        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            var auction = await _repository.GetAuctionWithBidsAsync(id);

            if (auction != null && auction.Bids != null)
            {
                auction.Bids = auction.Bids
                    .OrderByDescending(b => b.Amount)
                    .ToList();
            }

            return auction;
        }
        public async Task CreateAuctionAsync(Auction auction)
        {
            await _repository.CreateAuctionAsync(auction);
        }

        // Returns null on success, otherwise a user-friendly validation message.
        public async Task<string?> PlaceBidAsync(int auctionId, string bidderId, decimal amount)
        {
            var auction = await _repository.GetAuctionWithBidsAsync(auctionId);
            if (auction == null) return "Auktionen finns inte.";
            if (auction.SellerId == bidderId) return "Du kan inte lägga bud på din egen auktion.";
            if (auction.EndTime <= DateTime.Now) return "Auktionen har avslutats. Det går inte längre att lägga bud.";

            var highest = auction.Bids != null && auction.Bids.Any()
                ? auction.Bids.Max(b => b.Amount)
                : auction.StartPrice;
            if (amount <= highest)
                return $"Ditt bud måste vara högre än {highest:0.##} kr.";

            await _repository.AddBidAsync(new Bid
            {
                AuctionId = auctionId,
                BidderId = bidderId,
                Amount = amount,
                Created = DateTime.Now
            });
            return null;
        }

        public async Task UpdateDescriptionAsync(int id, string description)
        {
            var auction = await _repository.GetAuctionByIdAsync(id);
            if (auction == null)
                return;

            auction.Description = description;

            await _repository.UpdateAuctionAsync(auction);
        }
        public async Task<List<Auction>> GetMyActiveBidsAsync(string userId)
        {
            var auctions = await _repository.GetMyActiveBidsAsync(userId);

            foreach (var auction in auctions)
            {
                auction.Bids = auction.Bids
                    .OrderByDescending(b => b.Amount)
                    .ToList();
            }

            return auctions
                .OrderBy(a => a.EndTime)
                .ToList();
        }


        public async Task<List<Auction>> GetWonAuctionsAsync(string userId)
        {
            var auctions = await _repository.GetEndedAuctionsWithBidsAsync();

            var wonAuctions = auctions
                .Where(a =>
                {
                    var topBid = a.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();
                    return topBid != null && topBid.BidderId == userId;
                })
                .OrderByDescending(a => a.EndTime)
                .ToList();

            foreach (var auction in wonAuctions)
            {
                auction.Bids = auction.Bids
                    .OrderByDescending(b => b.Amount)
                    .ToList();
            }

            return wonAuctions;
        }

    }
}
