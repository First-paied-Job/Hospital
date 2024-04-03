namespace Hospital.Web.ViewModels.Administration.Dashboard.Room
{
    using System.ComponentModel.DataAnnotations;

    public class AddRoomToDepartmentInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string RoomType { get; set; }

        [Required]
        public string DepartmentId { get; set; }
    }
}
