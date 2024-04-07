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

            // Get departments that doctor is in
            var viewModel = await this.doctorService.GetDoctorsDepartmentsAsync(userId);
            return this.View(viewModel);
        }

        #region Room

        public async Task<IActionResult> RoomsInDepartment(string departmentId)
        {
            var doctorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            // Get rooms in department
            var viewModel = await this.doctorService.GetRoomsInDepartment(departmentId, doctorId);
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
                // Add patient to room
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
            // Remove patient
            await this.doctorService.RemovePatientFromRoomAsync(patientId, roomId);

            return this.Redirect("/Doctor/Dashboard");
        }

        #endregion

        #region Patient

        public async Task<IActionResult> PatientInfo(string patientId)
        {
            // Get view model for patient
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
                // Add illness to patient
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
            // Remove illness from patient
            await this.doctorService.RemoveIlnessFromPatient(illnessId, patientId);

            return this.Redirect($"/Doctor/Dashboard/PatientInfo?patientId={patientId}");
        }

        public async Task<IActionResult> EditPatient(string patientId)
        {
            // Get edit view model
            var viewModel = await this.doctorService.GetEditPatientAsync(patientId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientPost(EditPatientInputModel input)
        {
            try
            {
                // Edit patient
                await this.doctorService.EditPatientAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noPatient", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("EditPatient", input);
            }

            return this.Redirect($"/Doctor/Dashboard/PatientInfo?patientId={input.PatientId}");
        }

        public async Task<IActionResult> EditIllness(string illnessId)
        {
            // Get edit view model
            var viewModel = await this.doctorService.GetEditIllnessAsync(illnessId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditIllnessPost(EditIllnessInput input)
        {
            try
            {
                // Edit patient
                await this.doctorService.EditIllnessAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noIllness", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("EditIllness", input);
            }

            return this.Redirect($"/Doctor/Dashboard");
        }

        #endregion
    }
}
