using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentRepository _commentRepo ;
        private readonly IStockRepository _stockRepo ;

        public CommentController(ApplicationDbContext context, ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _context = context;
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }
        [HttpPost("stockId")]
        public async Task<IActionResult> Create([FromBody] int stockId, CreateCommentDto commentDto)
        {
            var isStockExist = await _stockRepo.StockExists(stockId);
            if(isStockExist == false)
            {
                return BadRequest("Stock does not exists");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel}, commentModel.ToCommentDto());
        }
    }
} 