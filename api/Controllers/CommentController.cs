using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public CommentController(ApplicationDbContext context, ICommentRepository repository)
        {
            _context = context;
            _commentRepo = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentDto);
        }
    }
} 