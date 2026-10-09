using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionLab.Data.Entities;
using AuctionLab.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionLab.Data.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _context;

        public AuctionRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auction>> GetActiveAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.EndTime > DateTime.Now)
                .Include(a => a.Bids)
                .OrderBy(a => a.EndTime)
                .ToListAsync();
        }

        public async Task<Auction?> GetAuctionWithBidsAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAuctionAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuctionAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Auction>> GetMyActiveBidsAsync(string userId)
        {
            var auctions = await _context.Auctions
                .Include(a => a.Bids)
                .Where(a => a.EndTime > DateTime.Now &&
                            a.Bids.Any(b => b.BidderId == userId))
                .ToListAsync();

            return auctions;
        }

        public async Task<List<Auction>> GetEndedAuctionsWithBidsAsync()
        {
            return await _context.Auctions
                .Include(a => a.Bids)
                .Where(a => a.EndTime <= DateTime.Now && a.Bids.Any())
                .ToListAsync();
        }

        public async Task AddBidAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
        }
    }
}
