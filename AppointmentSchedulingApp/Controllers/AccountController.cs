using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulingApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
