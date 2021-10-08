using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace MLDB.Api.Services
{
    public class SiteSurveyService : ISiteSurveyService {

        private readonly SiteSurveyContext _dbCtx;
        private readonly IUserService _userSvc;

        public SiteSurveyService(SiteSurveyContext context, IUserService userService) { 
            _dbCtx = context;
            _userSvc = userService;
        }

        public Task<List<Survey>> getAll() {
            return _dbCtx.Surveys.ToListAsync();
        }

        public async Task<Survey> getSurvey(Guid surveyId) {
            var survey = await _dbCtx.Surveys.FindAsync(surveyId);

            return survey;
        }

        public async Task<Survey> create(Survey survey, Guid siteId, ClaimsPrincipal userPrinciple) {
            var siteExists = _dbCtx.Sites.AnyAsync( x => x.Id == siteId );
            if( !await siteExists ) {
                throw new InvalidOperationException($"Site {siteId} does not exist");
            }

            var createUser = _userSvc.createFromClaimsPrinicpal(userPrinciple);

            var dbUser = _dbCtx.Users.Find(createUser.IdpId);
            
            survey.CreateUser = dbUser ?? createUser;
            survey.CreateTimestamp =  System.DateTime.UtcNow;
            survey.SiteId = siteId;

            _dbCtx.Surveys.Add(survey);

            await _dbCtx.SaveChangesAsync();

            return survey;
        }

        public async Task<Survey> update(Survey survey, Guid siteId, ClaimsPrincipal userPrinciple) {
            var createUser = _userSvc.createFromClaimsPrinicpal(userPrinciple);

            var dbUser = _dbCtx.Users.Find(createUser.IdpId);
            
            survey.CreateUser = dbUser ?? createUser;
            survey.SiteId = siteId;

            _dbCtx.Entry(survey).State = EntityState.Modified;

            // should probably handle the update exception here (like controller scaffold)
            await _dbCtx.SaveChangesAsync();

            return survey;
        }
    
        private bool SurveyExists(Guid id)
        {
            return _dbCtx.Surveys.Any(e => e.Id == id);
        }
    }
}