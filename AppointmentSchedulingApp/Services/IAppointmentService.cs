using AppointmentSchedulingApp.Models.ViewModels;

namespace AppointmentSchedulingApp.Services
{
    public interface IAppointmentService
    {
        public Task<List<DoctorVM>> GetDoctorList();
        public Task<List<PatientVM>> GetPatientList();

        public Task<int> AddUpdate(AppointmentVM model);
    }
}
