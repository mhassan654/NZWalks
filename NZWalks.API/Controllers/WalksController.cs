using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository= walkRepository;

        }
        // CREATE WALK
        // POST: /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTo addWalkRequestDTo)
        {
            //map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTo);
            await walkRepository.CreateAsync(walkDomainModel);

            // map domain model to dto
            return Ok(mapper.Map<WalkDTo>(walkDomainModel));
        }

        // get walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var walksDomainModel= await walkRepository.GetAllAsync();
           // map domain model to dto
           return Ok(mapper.Map<List<WalkDTo>>(walksDomainModel));
        }

        // get walk by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
           
            // map domain model to dto
            return Ok(mapper.Map<WalkDTo>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDTo updateWalkRequestDTo)
        {
            //map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDTo);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
           
            //map domain model to dto
            return Ok(mapper.Map<WalkDTo>(walkDomainModel));
        }

        //Delete a walk by id
        // Delete: /api/walks/{id}
        [HttpDelete]
        [Route(("{id:Guid}"))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomanModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomanModel == null)
            {
                return NotFound();
            }

            // map domain model to dto
            return Ok(mapper.Map<WalkDTo>(deletedWalkDomanModel));
        }
    }
}
