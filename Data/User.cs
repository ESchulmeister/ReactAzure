using System;
using System.Collections.Generic;

namespace reactAzure.Data
{
    public partial class User
    {
        public int UsrId { get; set; }
        public string UsrLogin { get; set; } = string.Empty;
        public string UsrFirst { get; set; } = string.Empty;
        public string UsrLast { get; set; } = string.Empty;
        public string UsrPassword { get; set; } =  string.Empty;
        public int UsrFlags { get; set; }
        public int? UsrClock { get; set; }
        public int? UsrApps { get; set; }
        public string? UsrCsdbName { get; set; }
        public string? UsrEmail { get; set; }
        public string? UsrCreatedBy { get; set; }
        public DateTime? UsrCreatedDate { get; set; }
        public string? UsrModifiedBy { get; set; }
        public DateTime? UsrModifiedDate { get; set; }
        public byte? UsrActive { get; set; }
        public byte[] UsrModifiedTimestamp { get; set; } = null!;
        public string? UsrNetLogin { get; set; }
        public string? UsrSettings { get; set; }
        public int? UsrDefaultRole { get; set; }
        public double? UsrFte { get; set; }

        public virtual Role? UsrDefaultRoleNavigation { get; set; }
    }
}
