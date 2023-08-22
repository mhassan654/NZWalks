using System.ComponentModel.DataAnnotations;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTOs
{
    public class AddWalkRequestDTo
    {
        [Required]
        [MaxLength(100,ErrorMessage = "Name must have 100 maximum characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description must have 200 maximum characters")]
        public string Description { get; set; }

        [Required]
        [Range(0,50)]
        public double LengthInKme { get; set; }
        public string? WalkImageUrl { get; set; }


        [Required]
        public Guid DifficultyId { get; set; }


        [Required]
        public Guid RegionId { get; set; }
    }
}
