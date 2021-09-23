using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLDB.Api.Models;
using System.Security.Claims;
using MLDB.Api.Services;

namespace MLDB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly SiteSurveyContext _context;
        private readonly IUserService _userSvc;
        private readonly ISiteService  _siteSvc;

        public SiteController(SiteSurveyContext context,  IUserService userService, ISiteService siteService)
        {
            _context = context;
            _userSvc = userService;
            _siteSvc = siteService;
        }

        // GET: api/Site
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            return await _siteSvc.getAll();
        }

        // GET: api/Site/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(Guid id)
        {
            var site = await _siteSvc.find(id);

            if (site == null)
            {
                return NotFound();
            }

            return site;
        }

        // PUT: api/Site/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSite(Guid id, Site site)
        {
            if (id != site.Id)
            {
                return BadRequest();
            }

            _context.Entry(site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
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

        // POST: api/Site
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite([FromBody]Site site)
        {
            var createdSite = await _siteSvc.create(site, HttpContext.User);
           
            return CreatedAtAction("GetSite", new { id = createdSite.Id }, createdSite);
        }

        // DELETE: api/Site/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Site>> DeleteSite(Guid id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();

            return site;
        }

        private bool SiteExists(Guid id)
        {
            return _context.Sites.Any(e => e.Id == id);
        }
    }
}
