using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTOs
{
    public class WalkDTo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKme { get; set; }
        public string? WalkImageUrl { get; set; }

        // Navigation properties
        public RegionDTo Region { get; set; }
        public DifficultyDTo Difficulty { get; set; }
    }
}
