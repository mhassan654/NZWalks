using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Controllers.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext _dbContext;
        private readonly IRegionsRepository _regionsRepository;
        private readonly IMapper _mapper;

        public RegionsController(
            NzWalksDbContext dbContext,
            IRegionsRepository regionsRepository,
            IMapper mapper
            )
        {
            this._dbContext = dbContext;
            this._regionsRepository = regionsRepository;
            this._mapper = mapper;
        }
        
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _regionsRepository.GetAllAsync();

            var regionsDto = _mapper.Map<List<RegionDTo>>(regionsDomain);

            return Ok(regionsDto);
        }
        
        // GET SINGLE REGION (Get Region by ID)
        //GET: https://localhost:portNumber/api/regions/id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await _regionsRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<RegionDTo>(regionDomain));
        }

        // POST TO CREATE NEW REGION
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidationModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTo addRegionRequestDto)
        {
                var regionDomainModal = _mapper.Map<Region>(addRegionRequestDto);

                // use domain modal to create region
                regionDomainModal = await _regionsRepository.CreateAsync(regionDomainModal);

                var regionDto = _mapper.Map<RegionDTo>(regionDomainModal);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
           
            
        }

        //UPDATE REGION
        //PUT: http://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidationModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTo updateRegionRequestD)
        {
                var regionDomainModal = _mapper.Map<Region>(updateRegionRequestD);

                // check if region exists
                regionDomainModal = await _regionsRepository.UpdateAsync(id, regionDomainModal);

                if (regionDomainModal == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<RegionDTo>(regionDomainModal));
         
        }

        //DELETE REGION
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModal =await _regionsRepository.DeleteAsync(id);
            if (regionDomainModal == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RegionDTo>(regionDomainModal));
        }
    }
}
