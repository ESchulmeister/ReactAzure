using System.ComponentModel.DataAnnotations;

namespace reactAzure.Models
{
    public class RoleModel
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public string Name { get; set; } = string.Empty;

        public bool IsSupervisor { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsActive { get; set; } = true;
        public bool WillTrackHours { get; set; } = false;
    }
}
