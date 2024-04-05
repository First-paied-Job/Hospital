namespace Hospital.Web.ViewModels.Directors.Schelude
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ScheludeInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Shift { get; set; }
    }
}
