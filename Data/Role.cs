using System;
using System.Collections.Generic;

namespace reactAzure.Data
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RId { get; set; }
        public string RName { get; set; } = null!;
        public bool RSupervisor { get; set; }
        public bool RAdministrator { get; set; }
        public bool? RActive { get; set; }
        public DateTime RCreatedDate { get; set; }
        public string RCreatedBy { get; set; } = null!;
        public DateTime RModifiedDate { get; set; }
        public string RModifiedBy { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
