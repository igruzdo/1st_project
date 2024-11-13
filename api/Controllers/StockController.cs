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
    }
} 