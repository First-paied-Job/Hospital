namespace Hospital.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Hospital.Web.ViewModels.Patients;

    public interface IPatientService
    {
        Task<IndexViewModel> GetInformationForPatient(string userId);
    }
}
