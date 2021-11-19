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
using MLDB.Infrastructure.Repositories;
using MLDB.Domain;

namespace MLDB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LitterTypeController : ControllerBase
    {
        private readonly SiteSurveyContext _dbCtx;
        
        public LitterTypeController(SiteSurveyContext ctx)
        {
            _dbCtx = ctx;
        }

        // GET: LitterItem
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<LitterType>>> GetLitterTypes()
        {
            var litterTypes =  await _dbCtx.LitterTypes.ToListAsync();

            return litterTypes;
        }
    }
}