using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MLDB.Api.Services;
using AutoMapper;
using MLDB.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using MLDB.Infrastructure.Repositories;
using MLDB.Domain;
using MLDB.Api.Mapping;

namespace MLDB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LitterTypeController : ControllerBase
    {
        private readonly ILitterTypeRepository _litterTypeRepo;
        private readonly IMapper _mapper;

        public LitterTypeController(ILitterTypeRepository litterTypeRepository, IMapper mapper)
        {
            _litterTypeRepo = litterTypeRepository;
            _mapper = mapper;
        }

        // GET: LitterType
        [HttpGet()]
        public async Task<ActionResult<List<LitterTypeDTO>>> GetLitterTypes()
        {
            var litterTypes =  await _litterTypeRepo.getAll();

            return _mapper.Map<List<LitterType>, List<LitterTypeDTO>>(litterTypes);
        }
    }
}