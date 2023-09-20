using AppointmentSchedulingApp.Services;
using AppointmentSchedulingApp.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulingApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _serivce;

        public AppointmentController(IAppointmentService service)
        {
            _serivce = service;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.DoctorList =  await _serivce.GetDoctorList();
            ViewBag.PatientList = await _serivce.GetPatientList();
            ViewBag.Duration = Helper.GetTimeDropDown();
            return View();
        }
    }
}
