using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MLDB.Api.Services;
using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Domain;
using MLDB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MLDB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class SiteController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userSvc;

        private readonly ISiteRepository _siteRepo;

        private readonly SiteSurveyContext _dbCtx;

        public SiteController(IUserService userService, IMapper mapper, ISiteRepository siteRepo, SiteSurveyContext ctx)
        {
            _mapper = mapper;
            _userSvc = userService;
            _siteRepo = siteRepo;
            _dbCtx = ctx;
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
                await _dbCtx.SaveChangesAsync();
            }
            catch (System.Data.RowNotInTableException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: /site
        [HttpPost]
        public async Task<ActionResult<SiteDTO>> PostSite([FromBody] SiteDTO siteDTO)
        {
            var requestUser = await _userSvc.findOrAddUserAsync(HttpContext.User);
            
            var siteToCreate = siteDTO with { CreatedBy = requestUser.IdpId };
            
            var site = _mapper.Map<Site>(siteToCreate);
            var createdSite = await _siteRepo.insertAsync(site);

            await _dbCtx.SaveChangesAsync();

            return CreatedAtAction("GetSite", new { id = createdSite.Id }, _mapper.Map<SiteDTO>(createdSite));
        }
    }
}
