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
using System.Data;

namespace MLDB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly ISiteService  _siteSvc;

        private readonly IUserService _userSvc;

        public SiteController(IUserService userService, IMapper mapper, ISiteService siteService)
        {
            _mapper  = mapper;
            _siteSvc = siteService;
            _userSvc = userService;
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
            var site = await _siteSvc.getSite(id);

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

            User requestUser = _userSvc.findOrAddUser(HttpContext.User);
            try
            {
                await _siteSvc.update(site, requestUser);
            }
            catch (System.Data.RowNotInTableException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: /site
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite([FromBody]SiteDTO siteDTO)
        {
            var site = _mapper.Map<Site>(siteDTO);

            User requestUser = _userSvc.findOrAddUser(HttpContext.User);

            var createdSite = await _siteSvc.create(site, requestUser);
           
            return CreatedAtAction("GetSite", new { id = createdSite.Id }, createdSite);
        }
    }
}
