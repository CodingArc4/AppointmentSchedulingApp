using Microsoft.AspNetCore.Identity;

namespace AppointmentSchedulingApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
