﻿using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulingApp.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
