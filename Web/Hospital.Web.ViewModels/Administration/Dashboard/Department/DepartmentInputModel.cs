﻿namespace Hospital.Web.ViewModels.Administration.Dashboard.Department
{
    using System.ComponentModel.DataAnnotations;

    public class DepartmentInputModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string HospitalEmployerId { get; set; }
    }
}
