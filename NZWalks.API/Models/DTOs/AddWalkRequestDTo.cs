using NZWalks.API.Models.Domains;

namespace NZWalks.API.Models.DTOs
{
    public class AddWalkRequestDTo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKme { get; set; }
        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }

        public Guid RegionId { get; set; }
        
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
