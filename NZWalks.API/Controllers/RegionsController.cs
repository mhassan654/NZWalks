using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name="Auckland Region",
                    Code="AKL",
                    RegionImageUrl="testing"
                }
            };

            return Ok(regions);
        }
    }
}
