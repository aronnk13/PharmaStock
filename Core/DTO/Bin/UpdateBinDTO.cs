using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Bin
{
    public class UpdateBinDTO
    {
        public int? BinStorageClassId { get; set; }

        public bool? IsQuarantine { get; set; }

        public bool? IsActive { get; set; }

        public int? MaxCapacity { get; set; }
    }
}
