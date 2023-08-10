using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionsRepository _regionsRepository;
        public RegionsController(NZWalksDbContext dbContext, IRegionsRepository regionsRepository)
        {
            this._dbContext = dbContext;
            this._regionsRepository = regionsRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _regionsRepository.GetAllAsync();

            // map domain models to DTos
            var regionsDto = new List<RegionDTo>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDTo()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            return Ok(regionsDto);
        }
        
        // GET SINGLE REGION (Get Region by ID)
        //GET: https://localhost:portNumber/api/regions/id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = _dbContext.Regions.Find(id);
            var regionDomain = await _regionsRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // map /convert region domain model to region dto
            var regionDto = new RegionDTo
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDomain);
        }

        // POST TO CREATE NEW REGION
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTo addRegionRequestDto)
        {
            // map or convert dto to domain modal
            var regionDomainModal = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // use domain modal to creare region
           regionDomainModal= await _regionsRepository.CreateAsync(regionDomainModal);

            // map domain modal back to dto
            var regionDto = new RegionDTo
            {
                Id = regionDomainModal.Id,
                Code = regionDomainModal.Code,
                Name = regionDomainModal.Name,
                RegionImageUrl = regionDomainModal.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        // Update region
        //PUT: http://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTo updateRegionRequestD)
        {
            // map dto to domain model
            var regionDomainModal = new Region
            {
                Code = updateRegionRequestD.Code,
                Name = updateRegionRequestD.Name,
                RegionImageUrl = updateRegionRequestD.RegionImageUrl
            };

            // check if region exists
            regionDomainModal = await _regionsRepository.UpdateAsync(id, regionDomainModal);


            if (regionDomainModal == null)
            {
                return NotFound();                
            }

            // Map DTO to domain modal
            regionDomainModal.Code = updateRegionRequestD.Code;
            regionDomainModal.Name = updateRegionRequestD.Name;
            regionDomainModal.RegionImageUrl = updateRegionRequestD.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            // convert domain model to dto
            var regionDto = new RegionDTo
            {
                Id = regionDomainModal.Id,
                Code = regionDomainModal.Code,
                Name = regionDomainModal.Name,
                RegionImageUrl = regionDomainModal.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // Delete REgion
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModal =await _regionsRepository.DeleteAsync(id);

            if (regionDomainModal == null)
            {
                return NotFound();
            }


            // convert domain model to dto
            var regionDto = new RegionDTo
            {
                Id = regionDomainModal.Id,
                Code = regionDomainModal.Code,
                Name = regionDomainModal.Name,
                RegionImageUrl = regionDomainModal.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
