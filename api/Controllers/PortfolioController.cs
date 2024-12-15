using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly UserManager<AppUser> _userManager;
        public  PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = User.GetUserName();
            var user = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(user);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetByIdSymbolAsync(symbol);

            if (stock == null) return BadRequest("Stock not found");

            var portfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);

            if (portfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id, 
            };

            if (portfolioModel == null) return StatusCode(500, "Could Not Create");

            await _portfolioRepository.CreateAsync(portfolioModel);

            return Created(); 
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeletePortfolio(string symbol)
        {
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);
            var portfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);

            var filteredPortfolio = portfolio.Where(e => e.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredPortfolio.Count() == 1)
            {
                await _portfolioRepository.DeletePortfolio(appUser,  symbol );
            }
            else
            {
                return BadRequest("Stock is not in your portfolio");
            }

            return Ok();  
        }
    }
}