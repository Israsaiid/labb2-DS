using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuctionLab.Business.Interfaces;
using AuctionLab.Data.Entities;
using AuctionLab.Web.Models;

namespace AuctionLab.Web.Controllers
{
    [Authorize]
    public class AuctionsController : Controller
    {
        private readonly IAuctionService _auctionService;
        private readonly UserManager<IdentityUser> _userManager;

        public AuctionsController(
            IAuctionService auctionService,
            UserManager<IdentityUser> userManager)
        {
            _auctionService = auctionService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var auctions = await _auctionService.GetActiveAuctionsAsync();

            var vm = auctions.Select(a => new ActiveAuctionVM
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartPrice = a.StartPrice,
                EndTime = a.EndTime,
                SellerId = a.SellerId,
                SellerName = _userManager.FindByIdAsync(a.SellerId).Result?.UserName ?? ""
            }).ToList();

            return View(vm);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            if (auction == null)
                return NotFound();

            var highestBid = auction.Bids.Any()
                ? auction.Bids.Max(b => b.Amount)
                : auction.StartPrice;

            var topBid = auction.Bids
                .OrderByDescending(b => b.Amount)
                .FirstOrDefault();

            var highestBidderName = topBid != null
                ? _userManager.FindByIdAsync(topBid.BidderId).Result?.UserName ?? ""
                : "";

            var sellerUser = await _userManager.FindByIdAsync(auction.SellerId);
            var sellerName = sellerUser?.UserName ?? "";

            var vm = new AuctionDetailsVM
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartPrice = auction.StartPrice,
                EndTime = auction.EndTime,
                HighestBid = highestBid,
                HighestBidderName = highestBidderName,
                SellerId = auction.SellerId,
                SellerName = sellerName,
                Bids = auction.Bids
                    .OrderByDescending(b => b.Amount)
                    .Select(b => new BidVM
                    {
                        Id = b.Id,
                        Amount = b.Amount,
                        Created = b.Created,
                        BidderId = b.BidderId,
                        BidderName = _userManager.FindByIdAsync(b.BidderId).Result?.UserName ?? ""
                    }).ToList()
            };


            return View(vm);
        }


        public IActionResult Create()
        {
            var vm = new CreateAuctionVM
            {
                EndTime = DateTime.Now.AddDays(1)
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuctionVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User)!;

            var auction = new Auction
            {
                Title = model.Title,
                Description = model.Description,
                StartPrice = model.StartPrice,
                EndTime = model.EndTime,
                SellerId = userId
            };

            await _auctionService.CreateAuctionAsync(auction);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceBid(int auctionId, decimal amount)
        {
            var userId = _userManager.GetUserId(User)!;

            await _auctionService.PlaceBidAsync(auctionId, userId, amount);

            return RedirectToAction(nameof(Details), new { id = auctionId });
        }

        public async Task<IActionResult> MyBids()
        {
            var userId = _userManager.GetUserId(User)!;
            var auctions = await _auctionService.GetMyActiveBidsAsync(userId);

            var vm = auctions.Select(a => new ActiveAuctionVM
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartPrice = a.StartPrice,
                EndTime = a.EndTime,
                SellerId = a.SellerId,
                SellerName = _userManager.FindByIdAsync(a.SellerId).Result?.UserName ?? ""
            }).ToList();

            return View(vm);
        }

        public async Task<IActionResult> WonAuctions()
        {
            var userId = _userManager.GetUserId(User)!;
            var auctions = await _auctionService.GetWonAuctionsAsync(userId);

            var vm = auctions.Select(a => new ActiveAuctionVM
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartPrice = a.StartPrice,
                EndTime = a.EndTime,
                SellerId = a.SellerId,
                SellerName = _userManager.FindByIdAsync(a.SellerId).Result?.UserName ?? ""
            }).ToList();

            return View(vm);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            var userId = _userManager.GetUserId(User);

            if (auction == null || auction.SellerId != userId)
                return Forbid();

            var vm = new EditAuctionDescriptionVM
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(EditAuctionDescriptionVM vm)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(vm.Id);
            var userId = _userManager.GetUserId(User);

            if (auction == null || auction.SellerId != userId)
                return Forbid();

            // Only description is updated – matches lab requirement
            await _auctionService.UpdateDescriptionAsync(vm.Id, vm.Description);

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }


    }
}
