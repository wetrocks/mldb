using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLDB.Api.Services;
using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Domain;
using MLDB.Infrastructure.Repositories;

namespace MLDB.Api.Controllers
{
    [Route("/site/{siteId}/survey")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService  _userSvc;

        private readonly ISurveyRepository _surveyRepo;

        private readonly SiteSurveyContext _dbCtx;

        public SurveyController(IUserService userService, ISurveyRepository surveyRepo, IMapper mapper, SiteSurveyContext ctx)
        {
            _userSvc = userService;
            _surveyRepo = surveyRepo;
            _mapper = mapper;
            _dbCtx = ctx;
        }

        // GET: /site/{siteid}/survey
        [HttpGet]
        public async Task<ActionResult<List<SurveyDTO>>> GetSurveys(Guid siteId)
        {
            var surveys = await _surveyRepo.getSurveysForSite(siteId);
            
            var dtos = _mapper.Map<List<Survey>, List<SurveyDTO>>(surveys);

            return dtos;
        }

        // GET: api/Survey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDTO>> GetSurvey(Guid siteId, Guid id)
        {
            var survey = await _surveyRepo.findAsync(id);

            if (survey == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<SurveyDTO>(survey);

            return dto;
        }

        // PUT: api/Survey/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurvey(Guid siteId, Guid id, SurveyDTO surveyDTO)
        {   
            if (id != surveyDTO.Id)
            {
                return BadRequest();
            }

            var requestUser = await _userSvc.findOrAddUserAsync(HttpContext.User);

            var survey = _mapper.Map<Survey>(surveyDTO);
            try
            {
                await _surveyRepo.updateAsync(survey);
                await _dbCtx.SaveChangesAsync();
            }
            catch (System.Data.RowNotInTableException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Survey
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost()]
        public async Task<ActionResult<SurveyDTO>> PostSurvey(Guid siteId, SurveyDTO surveyDTO)
        {
            var requestUser = _userSvc.findOrAddUserAsync(HttpContext.User);
            
            var surveyToCreate = surveyDTO with { SiteId = siteId };
            
            var survey = _mapper.Map<Survey>(surveyToCreate);

            var newSurvey = await _surveyRepo.insertAsync(survey);

            await _dbCtx.SaveChangesAsync();

            return CreatedAtAction("GetSurvey", new { siteId = newSurvey.SiteId, id = newSurvey.Id }, _mapper.Map<SurveyDTO>(newSurvey));
        }
    }
}
