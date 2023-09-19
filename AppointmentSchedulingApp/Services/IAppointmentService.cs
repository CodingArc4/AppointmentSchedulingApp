using AppointmentSchedulingApp.Models.ViewModels;

namespace AppointmentSchedulingApp.Services
{
    public interface IAppointmentService
    {
        public List<DoctorVM> GetDoctorList();
        public List<PatientVM> GetPatientList();
    }
}
