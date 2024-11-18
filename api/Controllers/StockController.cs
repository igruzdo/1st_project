using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stok;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var stocks = _context.Stocks.ToList().Select(s => s.ToStokDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStokDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStokRequestDto stokDto)
        {
            var stokModel = stokDto.ToStokFromCreateDto();
            _context.Add(stokModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stokModel.Id }, stokModel.ToStokDto()); 
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStokRequestDto updateDto)
        {
            var stokModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if(stokModel == null)
            {
                return NotFound();
            }

            stokModel.Symbol = updateDto.Symbol;
            stokModel.CompanyName = updateDto.CompanyName;
            stokModel.Purchase = updateDto.Purchase;
            stokModel.Industry = updateDto.Industry;
            stokModel.MarketCap = updateDto.MarketCap;
            stokModel.LastDiv = updateDto.LastDiv;
            
            _context.SaveChanges();

            return Ok(stokModel.ToStokDto());
        }

        [HttpDelete]
        [Route("id")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stokModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if(stokModel == null)
            {
                return NotFound();
            }
            
            _context.Stocks.Remove(stokModel);

            return NoContent();
        }
    }
} 