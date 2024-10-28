using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Rating, App.BLL.DTO.Rating> _mapper;
        private readonly UserManager<AppUser> _userManager;


        public RatingsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Rating, App.BLL.DTO.Rating>(autoMapper);
        }

        // GET: api/Ratings
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user/{advertisementId}")]
        public async Task<ActionResult<List<App.DTO.v1_0.Rating>>> GetRatings(Guid advertisementId)
        {
            var res = (
                    await _bll.Ratings.GetAllByAdvertisementAndUserIdAsync(
                        advertisementId, 
                        Guid.Parse(_userManager.GetUserId(User))
                    )
                )
                .Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
        }
        
        [HttpGet("advertisement/{advertisementId}")]
        public async Task<ActionResult<List<App.DTO.v1_0.Rating>>> GetRatingsByAdvertisementId(Guid advertisementId)
        {
            var res = (await _bll.Ratings.GetAllByAdvertisementIdAsync(advertisementId))
                .Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1_0.Rating>> GetRating(Guid id)
        {
            
            var Rating = _mapper.Map(await _bll.Ratings.FirstOrDefaultAsync(id));
            
            if (Rating == null)
            {
                return NotFound();
            }
            return Rating;
        }

        // PUT: api/Ratings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(Guid id, App.DTO.v1_0.Rating Rating)
        {
            if (id != Rating.Id)
            {
                return BadRequest();
            }
            
            var bllRating = _mapper.Map(Rating);
             _bll.Ratings.Update(bllRating);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ratings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DTO.v1_0.Rating>> PostRating(App.DTO.v1_0.Rating Rating)
        {
            var bllRating = _mapper.Map(Rating);
            bllRating!.Id = Guid.NewGuid();
            bllRating!.AppUserId = Guid.Parse(_userManager.GetUserId(User));
            _bll.Ratings.Add(bllRating);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = bllRating.Id }, _mapper.Map(bllRating));
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            var Rating = await _bll.Ratings.FirstOrDefaultAsync(id);
            if (Rating == null)
            {
                return NotFound();
            }

            await _bll.Ratings.RemoveAsync(Rating);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool RatingExists(Guid id)
        {
            return _bll.Ratings.ExistsAsync(id).Result;
        }
    }
}
