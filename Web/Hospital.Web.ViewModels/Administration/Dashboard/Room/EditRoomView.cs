using System.ComponentModel.DataAnnotations;

namespace Hospital.Web.ViewModels.Administration.Dashboard.Room
{
    public class EditRoomView
    {
        public string Name { get; set; }

        public string RoomType { get; set; }

        public string RoomId { get; set; }
    }
}
