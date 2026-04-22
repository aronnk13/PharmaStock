using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Audit
{
    public class AuditLog
    {
        public bool Result { get; set; }
        public string? Message { get; set; }
    }
}