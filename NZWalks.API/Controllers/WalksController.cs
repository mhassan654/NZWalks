using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Controllers.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this._mapper = mapper;
            this._walkRepository= walkRepository;
        }
        // CREATE WALK
        // POST: /api/walks
        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTo addWalkRequestDTo)
        {
                //map dto to domain model
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTo);
                await _walkRepository.CreateAsync(walkDomainModel);

                // map domain model to dto
                return Ok(_mapper.Map<WalkDTo>(walkDomainModel));
        }

        // get walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNo=2&pageSize=4
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNo = 1,
            [FromQuery] int? pageSize=1000
            )
        {
           var walksDomainModel= await _walkRepository.GetAllAsync(
               filterOn,
               filterQuery,
               sortBy,
               isAscending ?? true,
               pageNo,
               pageSize
               );
           // map domain model to dto
           return Ok(_mapper.Map<List<WalkDTo>>(walksDomainModel));
        }

        // get walk by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
           
            // map domain model to dto
            return Ok(_mapper.Map<WalkDTo>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidationModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDTo updateWalkRequestDTo)
        {
                //map dto to domain model
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTo);

                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                //map domain model to dto
                return Ok(_mapper.Map<WalkDTo>(walkDomainModel));
        }

        //Delete a walk by id
        // Delete: /api/walks/{id}
        [HttpDelete]
        [Route(("{id:Guid}"))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // map domain model to dto
            return Ok(_mapper.Map<WalkDTo>(deletedWalkDomainModel));
        }
    }
}
