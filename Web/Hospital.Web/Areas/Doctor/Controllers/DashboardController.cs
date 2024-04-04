namespace Hospital.Web.Areas.Doctor.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Doctors.Patient;
    using Hospital.Web.ViewModels.Doctors.Room;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : DoctorController
    {
        private readonly IDoctorService doctorService;

        public DashboardController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var viewModel = await this.doctorService.GetDoctorsDepartmentsAsync(userId);
            return this.View(viewModel);
        }

        #region Room

        public async Task<IActionResult> RoomsInDepartment(string departmentId)
        {
            var viewModel = await this.doctorService.GetRoomsInDepartment(departmentId);
            return this.View(viewModel);
        }

        public IActionResult AddPatientToRoom(string roomId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            this.ViewBag.roomId = roomId;
            this.ViewBag.doctorId = userId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientToRoom(AddPatientToRoomInput input)
        {
            try
            {
                await this.doctorService.AddPatientToRoomAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noRoom", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddPatientToRoom", input);
            }

            return this.Redirect("/Doctor/Dashboard");
        }

        public async Task<IActionResult> RemovePatientFromRoom(string patientId, string roomId)
        {
            await this.doctorService.RemovePatientFromRoomAsync(patientId, roomId);

            return this.Redirect("/Doctor/Dashboard");
        }

        #endregion

        #region Patient

        public async Task<IActionResult> PatientInfo(string patientId)
        {
            var viewModel = await this.doctorService.GetPatientInfo(patientId);
            return this.View(viewModel);
        }

        public IActionResult AddIlnessToPatient(string patientId)
        {
            this.ViewBag.patientId = patientId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddIlnessToPatient(AddIlnessToPatientInput input)
        {
            try
            {
                await this.doctorService.AddIllnesToPatientAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noPatient", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddIlnessToPatient", input);
            }

            return this.Redirect($"/Doctor/Dashboard/PatientInfo?patientId={input.PatientId}");
        }

        public async Task<IActionResult> RemoveIlnessFromPatient(string illnessId, string patientId)
        {
            await this.doctorService.RemoveIlnessFromPatient(illnessId, patientId);

            return this.Redirect($"/Doctor/Dashboard/PatientInfo?patientId={patientId}");
        }

        public async Task<IActionResult> EditPatient(string patientId)
        {
            var viewModel = await this.doctorService.GetEditPatientAsync(patientId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientPost(EditPatientInputModel input)
        {
            try
            {
                await this.doctorService.EditPatientAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noPatient", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddIlnessToPatient", input);
            }

            return this.Redirect($"/Doctor/Dashboard/PatientInfo?patientId={input.PatientId}");
        }

        #endregion
    }
}
