namespace Hospital.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Hospital.Web.ViewModels.Directors;
    using Hospital.Web.ViewModels.Directors.Schelude;

    public interface IDirectorService
    {
        public Task ChangeScheludeForDoctor(ScheludeInputModel input);

        public Task<IndexViewModel> GetDirectorInfo(string userId);

        public Task<PatientForDoctor> GetPatientsForDoctor(string doctorId);

        public Task<PatientsInDepartment> GetPatientsInDepartment(string departmentId);

        public Task<Schelude> SetScheludeForDoctors(string departmentId);
    }
}
