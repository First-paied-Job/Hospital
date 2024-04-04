namespace Hospital.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Web.ViewModels.Doctors;
    using Hospital.Web.ViewModels.Doctors.Patient;
    using Hospital.Web.ViewModels.Doctors.Room;

    public interface IDoctorService
    {
        public Task AddIllnesToPatientAsync(AddIlnessToPatientInput input);

        public Task AddPatientToRoomAsync(AddPatientToRoomInput input);

        public Task EditPatientAsync(EditPatientInputModel input);

        public Task<IndexViewModel> GetDoctorsDepartmentsAsync(string userId);

        public Task<EditPatientViewModel> GetEditPatientAsync(string patientId);

        public Task<PatientInfoViewModel> GetPatientInfo(string patientId);

        public Task<ICollection<RoomInDepartment>> GetRoomsInDepartment(string departmentId);

        public Task RemoveIlnessFromPatient(string illnessId, string patientId);

        public Task RemovePatientFromRoomAsync(string patientId, string roomId);
    }
}
