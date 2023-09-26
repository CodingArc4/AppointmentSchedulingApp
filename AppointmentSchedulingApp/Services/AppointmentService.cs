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
            return _context.Appointments.Where(x => x.PatientId == patientsId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved

            }).ToList();
        }

        public List<AppointmentVM> DoctorsEventById(string doctorsId)
        {
            return _context.Appointments.Where(x => x.DoctorId == doctorsId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved

            }).ToList();
        }

        //method to get appointment by id
        public AppointmentVM GetById(int id)
        {
            return _context.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = _context.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(), 
                DoctorName = _context.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()

            }).SingleOrDefault();
        }

        //method to delete an appointment
        public async Task<int> Delete(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        //method to confirm an appointment
        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
