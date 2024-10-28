using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using AutoMapper;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location> _mapper;

        public LocationsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location>(autoMapper);
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<List<App.DTO.v1_0.Location>>> GetLocations()
        {
            var res = (await _bll.Locations.GetAllAsync())
                .Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1_0.Location>> GetLocation(Guid id)
        {
            
            var location = _mapper.Map(await _bll.Locations.FirstOrDefaultAsync(id));
            
            if (location == null)
            {
                return NotFound();
            }
            return location;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(Guid id, App.DTO.v1_0.Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }
            
            var bllLocation = _mapper.Map(location);
             _bll.Locations.Update(bllLocation);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1_0.Location>> PostLocation(App.DTO.v1_0.Location location)
        {
            var bllLocation = _mapper.Map(location);
            bllLocation!.Id = Guid.NewGuid();
            _bll.Locations.Add(bllLocation);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = bllLocation.Id }, _mapper.Map(bllLocation));
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var location = await _bll.Locations.FirstOrDefaultAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            await _bll.Locations.RemoveAsync(location);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(Guid id)
        {
            return _bll.Locations.ExistsAsync(id).Result;
        }
    }
}
