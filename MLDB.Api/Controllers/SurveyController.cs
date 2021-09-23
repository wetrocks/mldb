using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLDB.Api.Models;
using MLDB.Api.Services;

namespace MLDB.Api.Controllers
{
    [Route("/site/{siteId}/survey")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly SiteSurveyContext _context;
        private readonly ISiteSurveyService  _surveySvc;

        public SurveyController(SiteSurveyContext context, ISiteSurveyService surveyService)
        {
            _context = context;
            _surveySvc = surveyService;
        }

        // GET: api/Survey
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Survey>>> GetSurveys()
        {
            return await _context.Surveys.ToListAsync();
        }

        // GET: api/Survey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Survey>> GetSurvey(Guid id)
        {
            var survey = await _context.Surveys.FindAsync(id);

            if (survey == null)
            {
                return NotFound();
            }

            return survey;
        }

        // PUT: api/Survey/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurvey(Guid id, Survey survey)
        {
            if (id != survey.Id)
            {
                return BadRequest();
            }

            _context.Entry(survey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        public async Task<ActionResult<Survey>> PostSurvey(Guid siteId, Survey survey)
        {
            var newSurvey = await _surveySvc.create(survey, siteId, HttpContext.User);

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
