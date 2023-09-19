using AppointmentSchedulingApp.Models;
using AppointmentSchedulingApp.Models.ViewModels;
using AppointmentSchedulingApp.Utility;

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
                           join userRoles in _context.UserRoles on users.Id equals userRoles.UserId
                           join Roles in _context.Roles.Where(x => x.Name == Helper.Doctor) on userRoles.RoleId 
                           equals Roles.Id
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
            var patient = (from users in _context.Users
                           join userRoles in _context.UserRoles on users.Id equals userRoles.UserId
                           join Roles in _context.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId
                           equals Roles.Id
                           select new PatientVM
                           {
                               Id = users.Id,
                               Name = users.Name

                           }).ToList();
            return patient;
        }
    }
}
