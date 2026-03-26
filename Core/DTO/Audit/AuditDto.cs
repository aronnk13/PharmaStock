using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO
{
    public class AuditDto
    {
        public int UserId { get; set; }

        public string? Action { get; set; }

        public string Resource { get; set; } = null!;

        public string? Metadata { get; set; }
    }
}