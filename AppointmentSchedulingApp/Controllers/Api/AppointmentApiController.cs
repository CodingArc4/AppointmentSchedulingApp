using AppointmentSchedulingApp.Models.ViewModels;
using AppointmentSchedulingApp.Services;
using AppointmentSchedulingApp.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentSchedulingApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _service;
        private readonly IHttpContextAccessor _accessor;
        private readonly string loginUserId;
        private readonly string role;

        public AppointmentApiController(IAppointmentService service,IHttpContextAccessor accessor)
        {
            _service = service;
            _accessor = accessor;
            loginUserId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        //Api endpoint for saving calendar data
        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _service.AddUpdate(data).Result;
                
                if(commonResponse.status == 1)
                {
                    commonResponse.message = Helper.apppointmentUpdated;
                } 
                
                if(commonResponse.status == 2)
                {
                    commonResponse.message = Helper.apppointmentAdded;
                }
            }
            catch(Exception ex) 
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
