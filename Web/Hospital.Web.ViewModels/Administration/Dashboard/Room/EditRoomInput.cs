using System.ComponentModel.DataAnnotations;

namespace Hospital.Web.ViewModels.Administration.Dashboard.Room
{
    public class EditRoomInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string RoomType { get; set; }

        [Required]
        public string RoomId { get; set; }
    }
}
