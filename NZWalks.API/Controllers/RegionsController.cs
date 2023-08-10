using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain = _dbContext.Regions.ToList();

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
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = _dbContext.Regions.Find(id);
            var regionDomain = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
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
        public IActionResult Create([FromBody] AddRegionRequestDTo addRegionRequestDto)
        {
            // map or convert dto to domain modal
            var regionDomainModal = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // use domain modal to creare region
            _dbContext.Regions.Add(regionDomainModal);
            _dbContext.SaveChanges();

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
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTo updateRegionRequestD)
        {
            // check if region exists
            var regionDomainModal = _dbContext.Regions.FirstOrDefault(region => region.Id == id);

            if (regionDomainModal == null)
            {
                return NotFound();                
            }

            // Map DTO to domain modal
            regionDomainModal.Code = updateRegionRequestD.Code;
            regionDomainModal.Name = updateRegionRequestD.Name;
            regionDomainModal.RegionImageUrl = updateRegionRequestD.RegionImageUrl;

            _dbContext.SaveChanges();

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
        public IActionResult Delete([FromRoute] Guid id)
        {

        }
    }
}
