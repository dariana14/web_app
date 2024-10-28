using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Advertisement, App.BLL.DTO.Advertisement> _mapper;

        /// <inheritdoc />
        public AdvertisementsController(AppDbContext context, IAppBLL bll, UserManager<AppUser> userManager,
            IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Advertisement, App.BLL.DTO.Advertisement>(autoMapper);
        }

        
        // GET: api/Advertisements
        /// <summary>
        /// Return all advertisements visible to current user
        /// </summary>
        /// <returns>list of Advertisement</returns>
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Advertisement>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisements()
        {
            var res = (await _bll.Advertisements.GetAllAsync(
                    Guid.Parse(_userManager.GetUserId(User))
                ))
                .Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
        }

        // GET: api/Advertisements/5
        /// <summary>
        /// Get appointment by id.
        /// </summary>
        /// <param name="id">Id parameter of appointment.</param>
        /// <returns>Appointment.</returns>
        [ProducesResponseType<App.DTO.v1_0.Advertisement>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1_0.Advertisement>> GetAdvertisement(Guid id)
        {
            
            var advertisement = _mapper.Map(await _bll.Advertisements.FirstOrDefaultAsync(id));
            
            if (advertisement == null)
            {
                return NotFound();
            }
            return advertisement;
        }

        // PUT: api/Advertisements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update advertisement by id.
        /// </summary>
        /// <param name="id">Id parameter of advertisement to change.</param>
        /// <param name="advertisement">Advertisement with changes.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutAdvertisement(Guid id, App.DTO.v1_0.Advertisement advertisement)
        {
            if (id != advertisement.Id)
            {
                return BadRequest();
            }
            
            var bllAdvertisement = _mapper.Map(advertisement);
            bllAdvertisement!.AppUserId = Guid.Parse(_userManager.GetUserId(User));

            var advertisement1 = _mapper.Map(await _bll.Advertisements.FirstOrDefaultAsync(id));
            
            if (advertisement1 == null)
            {
                return NotFound();
            }
            
            _bll.Advertisements.Update(bllAdvertisement);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisementExists(id))
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

        // POST: api/Advertisements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new advertisement.
        /// </summary>
        /// <param name="advertisement">Advertisement.</param>
        /// <returns>Created advertisement.</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Advertisement>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Advertisement>> PostAdvertisement(App.DTO.v1_0.Advertisement advertisement)
        {
            var bllAdvertisement = _mapper.Map(advertisement);
            bllAdvertisement!.AppUserId = Guid.Parse(_userManager.GetUserId(User));
            bllAdvertisement!.Id = Guid.NewGuid();
            _bll.Advertisements.Add(bllAdvertisement);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetAdvertisement", new { id = bllAdvertisement.Id }, _mapper.Map(bllAdvertisement));
        }

        // DELETE: api/Advertisements/5
        /// <summary>
        /// Delete advertisement by id.
        /// </summary>
        /// <param name="id">Id parameter of advertisement to delete.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteAdvertisement(Guid id)
        {
            var advertisement = await _bll.Advertisements.FirstOrDefaultAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }

            await _bll.Ratings.RemoveByAdvertisementIdAsync(advertisement.Id);
            await _bll.Advertisements.RemoveAsync(advertisement);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool AdvertisementExists(Guid id)
        {
            return _bll.Advertisements.ExistsAsync(id).Result;
        }
    }
}
