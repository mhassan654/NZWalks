using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;

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
            var regions = _dbContext.Regions.ToList();
            return Ok(regions);
        }
        
        // GET SINGLE REGION (Get Region by ID)
        //GET: https://localhost:portNumber/api/regions/id
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var region = _dbContext.Regions.Find(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);

        }
    }
}
