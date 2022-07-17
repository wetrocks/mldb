using System.Security.Claims;
using System.Threading.Tasks;
using System;
using MLDB.Api.DTO;
using MLDB.Domain;
using MLDB.Api.Mapping;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Globalization;
using System.Linq;

namespace MLDB.Api.Services
{
    public class SurveyService :  ISurveyService {

        private readonly IUserService _userSvc;
        private readonly ISiteRepository _siteRepo;
        private readonly ISurveyRepository _surveyRepo;

        private readonly ILogger<SurveyService> _logger;

        private readonly IMapper _mapper;

        public SurveyService(IUserService userService, ISiteRepository siteRepository, ISurveyRepository surveyRepository,
                            ILogger<SurveyService> logger)
        {
            _userSvc = userService;
            _siteRepo = siteRepository;
            _surveyRepo  = surveyRepository;
            _logger = logger;

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new SurveyProfile()));
            _mapper = configuration.CreateMapper();
        }

        public async Task<SurveyDTO> createSiteSurvey(ClaimsPrincipal principal, Guid siteId, SurveyDTO surveyDTO) {
            var user = await _userSvc.findOrAddUserAsync(principal);

            var survey = this.createSurveyFromDTO(surveyDTO, siteId, user.IdpId);

            var createdSurvey = await _surveyRepo.insertAsync(survey);

            return _mapper.Map<SurveyDTO>(survey);
        }

        public async Task<SurveyDTO> updateSiteSurvey(ClaimsPrincipal principal, SurveyDTO surveyDTO) {
            var user = await _userSvc.findOrAddUserAsync(principal);
            
            var survey = this.createSurveyFromDTO(surveyDTO, surveyDTO.SiteId, user.IdpId, surveyDTO.Id);

            var updatedSurvey = await _surveyRepo.updateAsync(survey);

            return _mapper.Map<SurveyDTO>(survey);
        }

        private Survey createSurveyFromDTO(SurveyDTO dto, Guid siteId, string createUserId, Guid? surveyId = null) {
            var survey = surveyId != null ? new Survey(siteId, surveyId.Value, createUserId) : new Survey(siteId, createUserId);

            survey.CoordinatorName = dto.Coordinator;
            survey.VolunteerCount = dto.VolunteerCount;
            survey.TotalKg = dto.TotalKg;
            survey.SurveyDate = DateOnly.ParseExact(dto.SurveyDate, "yyyy-MM-dd");
            
            if( !String.IsNullOrWhiteSpace(dto.StartTime) ) {
                survey.StartTime = TimeOnly.ParseExact(dto.StartTime, "HH:mm:ss");
            }

            if( !String.IsNullOrWhiteSpace(dto.EndTime) ) {
                survey.EndTime = TimeOnly.ParseExact(dto.EndTime, "HH:mm:ss");
            }
 
            survey.updateLitterItems(dto.LitterItems.Select( x => new LitterItem(int.Parse(x.Key), x.Value)).ToList());

            return survey;
        }
    }
}