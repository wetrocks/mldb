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
using AutoMapper;
using MLDB.Api.DTO;
using Microsoft.AspNetCore.Authorization;

namespace MLDB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly SiteSurveyContext _context;

        private readonly IMapper _mapper;

        private readonly ISiteService  _siteSvc;

        public SiteController(SiteSurveyContext context, IMapper mapper, ISiteService siteService)
        {
            _context = context;
            _mapper  = mapper;
            _siteSvc = siteService;
        }

        // GET: /site
        [HttpGet]
        public async Task<ActionResult<List<SiteDTO>>> GetSites()
        {
            var sites =  await _siteSvc.getAll();
            
            return _mapper.Map<IList<Site>, List<SiteDTO>>(sites);
        }

        // GET: /site/<guid>
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteDTO>> GetSite(Guid id)
        {
            var site = await _siteSvc.find(id);

            if (site == null)
            {
                return NotFound();
            }

            return _mapper.Map<SiteDTO>(site);
        }

        // PUT: /site/<guid>
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutSite(Guid id, SiteDTO site)
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

        // POST: /site
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite([FromBody]SiteDTO siteDTO)
        {
            var site = _mapper.Map<Site>(siteDTO);

            var createdSite = await _siteSvc.create(site, HttpContext.User);
           
            return CreatedAtAction("GetSite", new { id = createdSite.Id }, createdSite);
        }


        private bool SiteExists(Guid id)
        {
            return _context.Sites.Any(e => e.Id == id);
        }
    }
}
