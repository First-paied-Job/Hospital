namespace Hospital.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Web.ViewModels.Administration.Dashboard.Department;
    using Hospital.Web.ViewModels.Administration.Dashboard.Director;
    using Hospital.Web.ViewModels.Administration.Dashboard.Doctor;
    using Hospital.Web.ViewModels.Administration.Dashboard.Hospital;
    using Hospital.Web.ViewModels.Administration.Dashboard.Room;
    using Hospital.Web.ViewModels.Administration.Dashboard.Statistics;
    using Hospital.Web.ViewModels.Administration.Dashboard.User;

    public interface IAdministrationService
    {
        #region User
        public Task<ICollection<UserRoleViewModel>> GetAllUsers();

        public Task AddRoleToUser(string roleId, string userId);

        public Task RemoveRoleFromUser(string roleId, string userId);

        #endregion

        #region Doctor

        public Task AddDoctorAsync(AddDoctorInput input);

        public Task RemoveDoctorAsync(string doctorId);

        public Task<ICollection<DoctorViewModel>> GetDoctorsAsync();

        public Task<EditDoctorViewModel> GetDoctorEditAsync(string doctorId);

        public Task EditDoctorAsync(EditDoctorInputModel input);

        #endregion

        #region Director

        public Task AddDirectorAsync(AddDirectorInput input);

        public Task RemoveDirectorAsync(string directorId);

        public Task<ICollection<DirectorViewModel>> GetDirectorsAsync();

        public Task<EditDirectorViewModel> GetDirectorEditAsync(string directorId);

        public Task EditDirectorAsync(EditDirectorInputModel input);

        #endregion

        #region Hospital

        public Task AddHospitalAsync(HospitalInputModel input);

        public Task RemoveHospitalAsync(string hospitalId);

        public Task<ICollection<HospitalViewModel>> GetHospitalsAsync();

        public Task<EditHospitalViewModel> GetHospitalEditAsync(string hospitalId);

        public Task EditHospitalAsync(EditHospitalInputModel input);

        #endregion

        #region Department

        public Task AddDepartmentToHospitalAsync(DepartmentInputModel input);

        public Task RemoveDepartmentAsync(string departmentId);

        public Task<ICollection<DepartmentViewModel>> GetDepartmentsInHospitalAsync(string hospitalId);

        public Task EditDepartmentAsync(EditDepartmentInputModel input);

        public Task<EditDepartmentViewModel> GetDepartmentEdit(string id);

        public Task AddDoctorToDepartment(AddDoctorToDepartmentInput input);

        public Task RemoveDoctorFromDepartment(string doctorId, string departmentId);

        public Task MakeDoctorBossOfDepartment(string doctorId, string departmentId);

        public Task RemoveDoctorBossOfDepartment(string doctorId, string departmentId);

        #endregion

        #region Room

        public Task AddRoomToDepartment(AddRoomToDepartmentInput input);

        public Task RemoveRoomFromDepartment(string roomId, string departmentId);

        #endregion

        #region Statistics

        public Task<ICollection<DoctorsViewModel>> GetPatientsStatisticsAsync();

        public Task<ICollection<DepartmentsViewModel>> GetDoctorsStatisticsAsync();

        #endregion
    }
}
