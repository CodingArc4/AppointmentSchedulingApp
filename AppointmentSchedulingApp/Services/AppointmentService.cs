using AppointmentSchedulingApp.Models;
using AppointmentSchedulingApp.Models.ViewModels;

namespace AppointmentSchedulingApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get list of All Doctors
        public List<DoctorVM> GetDoctorList()
        {
            var doctors = (from users  in _context.Users
                           select new DoctorVM
                           {
                               Id = users.Id,
                               Name = users.Name
                           
                           }).ToList();
            return doctors;
        }

        //Get List of All Patients
        public List<PatientVM> GetPatientList()
        {
            throw new NotImplementedException();
        }
    }
}
