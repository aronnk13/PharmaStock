namespace PharmaStock.Core.DTO.Location
{
    public class GetLocationDTO
    {
        public int LocationId { get; set; }
        public string Name { get; set; } = null!;
        public int LocationTypeId { get; set; }
        public int? ParentLocationId { get; set; }
        public bool StatusId { get; set; }
    }
}
