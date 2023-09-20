$(document).ready(function () {
    InitializeCalendar();
});

//function to display the  calendar
function InitializeCalendar(){
    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                }
            });
            calendar.render();
        }
    } catch(e) {
        alert(e);
    }
}

//function to open modal when clicked on a date
function onShowModal(obj,isEventDetail) {
    $('#appointmentInput').modal("show");
}

//function to close modal through close button
function onCloseModal() {
    //debugger
    $("#appointmentInput").modal("hide");
} 

//function to submit form
function onSubmitForm() {
    var requestData = {
        Id: parseInt($("#id").val()),
        Title: $("#title").val(),
        Description: $("#description").val(),
        StartDate: $("#appointmentDate").val(),
        Duration: $("#duration").val(),
        DoctorId: $("#doctorId").val(),
        PatientId: $("#patientId").val()
    }
}