﻿using AppointmentSchedulingApp.Models.ViewModels;

namespace AppointmentSchedulingApp.Services
{
    public interface IAppointmentService
    {
        public Task<List<DoctorVM>> GetDoctorList();
        public Task<List<PatientVM>> GetPatientList();
        public Task<int> AddUpdate(AppointmentVM model);
        public List<AppointmentVM> DoctorsEventById(string doctorsId);
        public List<AppointmentVM> PatientsEventById(string patientsId);
        public AppointmentVM GetById(int id);
        public Task<int> Delete(int id);
        public Task<int> ConfirmEvent(int id);
    }
}
