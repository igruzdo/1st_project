using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PortfolioRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _applicationDbContext.Portfolios.AddAsync(portfolio);
            await _applicationDbContext.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser user, string symbol)
        {
            var portfolioModel = await _applicationDbContext.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == user.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if(portfolioModel == null) return null;

            _applicationDbContext.Portfolios.Remove(portfolioModel);
            await _applicationDbContext.SaveChangesAsync();
            return portfolioModel; 
        }

        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {
            return await _applicationDbContext.Portfolios.Where(p => p.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
            }).ToListAsync();  
        }
    }
}