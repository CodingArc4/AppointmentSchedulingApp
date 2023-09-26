using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentSchedulingApp.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        public static string apppointmentAdded = "Appointment added successfully.";
        public static string apppointmentUpdated = "Appointment updated successfully.";
        public static string apppointmentDeleted = "Appointment deleted successfully.";
        public static string apppointmentExists = "Appointment for selected time and date already exists.";
        public static string apppointmentNotExists = "Appointment does not exists.";
      
        public static string MeetingConfirm = "Meeting Confirmed Successfully.";
        public static string MeetingConfirmError = "Something went wrong when confirming meeting";

        public static string apppointmentAddError = "Something went wrong while trying to add appointment,please try again.";
        public static string apppointmentUpdateError = "Something went wrong while trying to update appointment,please try again.";
        public static string somtimgWentWrong = "Something went wrong,please try again.";

        public static int success_code = 1;
        public static int failure_code = 0;

        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin) {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value = Helper.Admin,Text=Helper.Admin}
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value = Helper.Patient,Text=Helper.Patient},
                    new SelectListItem{Value = Helper.Doctor,Text=Helper.Doctor}
                };
            }
       
        }

        //function for the time drop down list
        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " Hr " });
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " Hr 30 min" });
                minute = minute + 30;
            }
            return duration;
        }
    }
}
