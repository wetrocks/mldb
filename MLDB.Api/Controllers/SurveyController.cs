using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLDB.Api.Models;
using MLDB.Api.Services;
using AutoMapper;
using MLDB.Api.DTO;
using Microsoft.AspNetCore.Authorization;

namespace MLDB.Api.Controllers
{
    [Route("/site/{siteId}/survey")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SiteSurveyContext _context;
        private readonly IUserService  _userSvc;
        private readonly ISiteSurveyService  _surveySvc;

        public SurveyController(SiteSurveyContext context, IUserService userService, ISiteSurveyService surveyService, IMapper mapper)
        {
            _context = context;
            _userSvc = userService;
            _surveySvc = surveyService;
            _mapper = mapper;
        }

        // GET: api/Survey
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Survey>>> GetSurveys()
        {
            return await _context.Surveys.ToListAsync();
        }

        // GET: api/Survey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDTO>> GetSurvey(Guid siteId, Guid id)
        {
            var survey = await _context.Surveys.FindAsync(id);

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

            User requestUser = _userSvc.findOrAddUser(HttpContext.User);

            var dto = _mapper.Map<Survey>(surveyDTO);
            try
            {
                await _surveySvc.update(dto, siteId, requestUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyExists(id))
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

        // POST: api/Survey
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost()]
        public async Task<ActionResult<Survey>> PostSurvey(Guid siteId, SurveyDTO surveyDTO)
        {
            var dto = _mapper.Map<Survey>(surveyDTO);
           
            User requestUser = _userSvc.findOrAddUser(HttpContext.User);

            var newSurvey = await _surveySvc.create(dto, siteId, requestUser);

            return CreatedAtAction("GetSurvey", new { siteId = newSurvey.SiteId, id = newSurvey.Id }, newSurvey);
        }

        // DELETE: api/Survey/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Survey>> DeleteSurvey(Guid id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();

            return survey;
        }

        private bool SurveyExists(Guid id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }
    }
}
