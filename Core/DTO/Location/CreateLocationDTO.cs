using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Location
{
    public class CreateLocationDTO
    {
        [Required(ErrorMessage = "Location name is required.")]
        [StringLength(255, ErrorMessage = "Location name cannot exceed 255 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Location type is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Location type must be a valid ID greater than 0.")]
        public int LocationTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Parent location ID must be greater than 0 if provided.")]
        public int? ParentLocationId { get; set; }

        public bool StatusId { get; set; } = true;
    }
}
