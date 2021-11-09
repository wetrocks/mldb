using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MLDB.Api.Services;
using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Domain;

namespace MLDB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userSvc;

        private readonly ISiteRepository _siteRepo;

        public SiteController(IUserService userService, IMapper mapper, ISiteRepository siteRepo)
        {
            _mapper = mapper;
            _userSvc = userService;
            _siteRepo = siteRepo;
        }

        // GET: /site
        [HttpGet]
        public async Task<ActionResult<List<SiteDTO>>> GetSites()
        {
            var sites = await _siteRepo.getAll();

            return _mapper.Map<IList<Site>, List<SiteDTO>>(sites);
        }

        // GET: /site/<guid>
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteDTO>> GetSite(Guid id)
        {
            var site = await _siteRepo.findAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return _mapper.Map<SiteDTO>(site);
        }

        // PUT: /site/<guid>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutSite(Guid id, SiteDTO siteDTO)
        {
            if (id != siteDTO.Id)
            {
                return BadRequest();
            }

            var site = _mapper.Map<Site>(siteDTO);

            //     MLDB.Api.Models.User requestUser = _userSvc.findOrAddUser(HttpContext.User);
            try
            {
                await _siteRepo.updateAsync(site);
            }
            catch (System.Data.RowNotInTableException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: /site
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite([FromBody] SiteDTO siteDTO)
        {
            var site = _mapper.Map<Site>(siteDTO);

            // MLDB.Api.Models.User requestUser = _userSvc.findOrAddUser(HttpContext.User);

            var createdSite = await _siteRepo.insertAsync(site);

            return CreatedAtAction("GetSite", new { id = createdSite.Id }, createdSite);
        }
    }
}
