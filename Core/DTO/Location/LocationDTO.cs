using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Location
{
    public class LocationDTO
    {
        public int LocationId { get; set; }

        public string Name { get; set; } = null!;

        public int LocationTypeId { get; set; }

        public int? ParentLocationId { get; set; }

        public bool StatusId { get; set; }
    }
}