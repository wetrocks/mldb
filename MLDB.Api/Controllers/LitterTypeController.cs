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
    public class LitterTypeController : ControllerBase
    {
        public LitterTypeController()
        {
        }

        // GET: LitterItem
        [HttpGet("litterTypes")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LitterTypeDTO>>> GetLitterTypes()
        {
            var litterTypes = new List<LitterTypeDTO>() {
                new LitterTypeDTO { Id = 42, OsparID =  42, DadID = "17", Description="Bags"},
                new LitterTypeDTO { Id = 43, OsparID =  43, DadID = "18", Description="Caps/Lids"}
            };
            
            return await Task.Run(() => litterTypes);
        }
    }
}