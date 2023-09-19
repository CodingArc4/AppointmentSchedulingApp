using AppointmentSchedulingApp.Services;
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

        public IActionResult Index()
        {
            _serivce.GetDoctorList();
            return View();
        }
    }
}
