﻿using System.ComponentModel.DataAnnotations;

namespace reactAzure.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Role = new RoleModel();  //FK   
        }

        public int ID { get; set; }
        public string? Login { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public string FullName => $"{this.FirstName} {this.LastName}";
        public string FullNameReversed => $"{this.LastName}, {this.FirstName}";
        [Required(ErrorMessage = "Role is Required")]
        public int? RoleID { get; set; }

        [Required(ErrorMessage = "FTE is Required")]
        public double? FTE { get; set; }
        public string? Clock { get; set; }
        public bool? WillTrackHours { get; set; } = false;
        public virtual RoleModel Role { get; set; }

    }
}
