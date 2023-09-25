using AppointmentSchedulingApp.Models;
using AppointmentSchedulingApp.Models.ViewModels;
using AppointmentSchedulingApp.Utility;
using Microsoft.AspNetCore.Identity;

namespace AppointmentSchedulingApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentService(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //method to Add/Update an appointment
        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if(model != null && model.Id > 0)
            {
                //update an existing appointment
                return 1;
            }
            else
            {
                //create a new appointment
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId

                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return 2;
            }
        }

        //Get list of All Doctors
        public async Task<List<DoctorVM>> GetDoctorList()
        {
            var doctors = await _userManager.GetUsersInRoleAsync(Utility.Helper.Doctor);
            
            var doctorVM = doctors.Select(user => new DoctorVM {
                   Id = user.Id,
                   Name = user.Name
                           
            }).ToList();
            return doctorVM;
        }

        //Get List of All Patients
        public async Task<List<PatientVM>> GetPatientList()
        {
            var patient = await _userManager.GetUsersInRoleAsync(Utility.Helper.Patient);

            var patientVM = patient.Select(user => new PatientVM 
            {
                Id = user.Id,
                Name = user.Name

            }).ToList();
            return patientVM;
        }

        public List<AppointmentVM> PatientsEventById(string patientsId)
        {
            return _context.Appointments.Where(x => x.PatientId == patientsId).ToList().Select(x => new AppointmentVM()
            {
                Id = x.Id,
                Description = x.Description,
                StartDate = x.StartDate.ToString("yyy-MM-dd HH:mm:ss"),
                EndDate = x.EndDate.ToString("yyy-MM-dd HH:mm:ss"),
                Title = x.Title,
                Duration = x.Duration,
                IsDoctorApproved = x.IsDoctorApproved

            }).ToList();
        }

        public List<AppointmentVM> DoctorsEventById(string doctorsId)
        {
            return _context.Appointments.Where(x => x.DoctorId == doctorsId).ToList().Select(x => new AppointmentVM()
            {
                Id = x.Id,
                Description = x.Description,
                StartDate = x.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = x.Title,
                Duration = x.Duration,
                IsDoctorApproved = x.IsDoctorApproved

            }).ToList();
        }
    }
}
