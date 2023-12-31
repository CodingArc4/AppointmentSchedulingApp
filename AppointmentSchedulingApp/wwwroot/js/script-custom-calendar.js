﻿var routeURL = location.protocol + "//" + location.host;

$(document).ready(function () {
    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });

    InitializeCalendar();
});


var calendar;
function InitializeCalendar() {
    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
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
                },
                //ajax api call to get calendar data and show it on the frontend.
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeURL + '/api/Appointment/GetCalendarData?doctorId=' + $("#doctorId").val(),
                        type: 'GET',
                        contentType: 'json',
                        success: function (response) {
                            var events = [];
                          
                            if (response.status === 1) {
                                $.each(response.dataenum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.isDoctorApproved ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id
                                    })
                                })
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                eventDisplay: 'block',
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
                }
            });
            calendar.render();
        }

    }
    catch (e) {
        alert(e);
    }
}


//function to open modal when clicked on a date
function onShowModal(obj, isEventDetail) {
    //fill the modal with the inserted data
    if (isEventDetail != null) {

        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $("#duration").val(obj.duration);
        $("#doctorId").val(obj.doctorId);
        $("#patientId").val(obj.patientId);
        $("#id").val(obj.id);
        $("#lblPatientName").html(obj.patientName);
        $("#lblDoctorName").html(obj.doctorName);

        if (obj.isDoctorApproved) {
            $("#lblStatus").html("Approved");
            $("#btnConfirm").addClass("d-none");
            $("#btnSubmit").addClass("d-none");
        }
        else {
            $("#lblStatus").html("Pending");
            $("btnConfirm").removeClass("d-none");
            $("btnDelete").removeClass("d-none");
        }
        $("btnDelete").removeClass("d-none");

    //getting a calendar date whenever a user click on a date to schedule an appointment
    } else {
        $("#appointmentDate").val(obj.startStr + " " + new moment().format("hh:mm A"));
        $("#id").val(0);
        $("btnDelete").addClass("d-none");
        $("btnSubmit").removeClass("d-none");
    }
    $('#appointmentInput').modal("show");
}

//function to close modal through close button
function onCloseModal() {
    $("#appointmentForm")[0].reset();
    $("#id").val(0);
    $("#title").val('');
    $("#description").val('');
    $("#appointmentDate").val('');
    $("#appointmentInput").modal("hide");
} 


//function to submit form
function onSubmitForm() {
    if (checkValidation()) {
        var requestData = {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#appointmentDate").val(),
            Duration: $("#duration").val(),
            DoctorId: $("#doctorId").val(),
            PatientId: $("#patientId").val(),
        };

        //ajax api call to insert data into the calendar
        $.ajax({
            url: routeURL + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    calendar.refetchEvents();
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
}


//function to check validation
function checkValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").addClass('error');
    }
    else {
        $("#appointmentDate").removeClass('error');
    }

    return isValid;
}


//function to get event details by id
function getEventDetailsByEventId(info) {
    $.ajax({
        url: routeURL + '/api/Appointment/GetCalendarDataById/' + info.id,
        type: 'GET',
        contentType: 'json',
        success: function (response) {
            if (response.status === 1 && response.dataenum !== undefined) {
                onShowModal(response.dataenum,true)
            }
            successCallback(events);
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

//show different doctors appointments
function onDoctorChange() {
    calendar.refetchEvents();
}

//function to delete appointment
function onDeleteAppointment() {
    var id = parseInt($("#id").val())
    $.ajax({
        url: routeURL + '/api/Appointment/DeleteAppointment/' + id,
        type: 'GET',
        contentType: 'json',
        success: function (response) {
            if (response.status === 1) {
                $.notify(response.message, "success")
                calendar.refetchEvents();
                onCloseModal(response.dataenum, true)
            }
            else {
                $.notify(response.message, "error")
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

//fucntion to confirm an appointment
function onConfirm() {
    var id = parseInt($("#id").val())
    $.ajax({
        url: routeURL + '/api/Appointment/ConfirmEvent/' + id,
        type: 'GET',
        contentType: 'json',
        success: function (response) {
            if (response.status === 1) {
                $.notify(response.message, "success")
                calendar.refetchEvents();
                onCloseModal(response.dataenum, true)
            }
            else {
                $.notify(response.message, "error")
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}