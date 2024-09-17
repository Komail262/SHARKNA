using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SHARKNA.Controllers
{
    [Area("Admin")]

    public class EventAttendenceController : Controller
    {
        private readonly EventAttendenceDomain _eventattendenceDomain;
        private readonly EventRegistrationsDomain _EventRegistrations;
        private readonly UserDomain _UserDomain;
        private readonly SHARKNAContext _context;



        public EventAttendenceController(EventAttendenceDomain eventAttendenceDomain, EventRegistrationsDomain eventRegDomain, UserDomain userDomain, SHARKNAContext context)
        {
            _eventattendenceDomain = eventAttendenceDomain;
            _EventRegistrations = eventRegDomain;
            _UserDomain = userDomain;
            _context = context;
        }
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> Index()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username of the logged-in user

            var events = await _eventattendenceDomain.GetTblEventsAsync();//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
            return View(events);
        }

        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> Details(Guid id)
        {
            var eventDays = await _eventattendenceDomain.GetEventDaysByEventIdAsync(id);//نطلع معلومات الفعالية عن طريق اليوم بشكل مقسم فنستدعي الايدي الموجود بالدومين ليعرض الايام لي
            if (eventDays == null)
            {
                return NotFound();
            }

            return View(eventDays);
        }
        [Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]

        public async Task<IActionResult> Members(Guid id, int day)
        {
            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);//ناخذ قائمة المسجلين يالفعاليات من الدومين ايفنت اتيندينس 

            return View(Ereg);

        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Members(IFormCollection forms, Guid id, int day)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var username = User.FindFirst(ClaimTypes.Name)?.Value;

        //            // Retrieve the IDs of attendees whose checkboxes were checked in the form
        //            var checkedAttendees = forms.Keys
        //                .Where(k => k.StartsWith("attendanceStatus["))
        //                .Select(k => Guid.Parse(k.Split('[')[1].TrimEnd(']')))
        //                .ToList();

        //            var count = checkedAttendees.Count;

        //            // Process the attendance status
        //            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);

        //            // Update attendance
        //            foreach (var attendee in Ereg)
        //            {
        //                bool isAttending = checkedAttendees.Contains(attendee.Id);
        //                _eventattendenceDomain.SetAttendanceStatus(attendee.Id, isAttending);
        //            }

        //            if (count > 0)
        //            {
        //                ViewData["Successful"] = "تم التحضير بنجاح";
        //            }
        //            else
        //            {
        //                ViewData["Falied"] = "حدث خطأ";
        //            }

        //            return View(Ereg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ";
        //        // Log the exception to understand what went wrong
        //    }
        //    var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
        //    return View(attendees);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Members(IFormCollection forms, Guid id, int day)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //var eventId = new Guid(forms["eventId"]);
                    var username = User.FindFirst(ClaimTypes.Name)?.Value;


                    var count = forms["attendanceStatus"].Count;



                    var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
                                                                                                //return View(Ereg);

                    if (count == 1)
                        ViewData["Successful"] = "تم التحضير بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ";

                    return View(Ereg);

                }
            }

            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ";
            }
            var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
            return View(attendees); // Return the correct model

            //return View(forms);

        }

        //[HttpPost] // sweet alert on
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Members(IFormCollection forms, Guid id, int day)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var username = User.FindFirst(ClaimTypes.Name)?.Value;

        //            // Process the form data using IFormCollection
        //            var attendanceStatus = forms["attendanceStatus"]; // Get the attendance status from the form
        //            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day); // Get the event attendees

        //            foreach (var attendee in Ereg)
        //            {
        //                // Check if the checkbox for the attendee is present in the form
        //                if (attendanceStatus.Contains(attendee.Id.ToString()))
        //                {
        //                    attendee.IsAttend = true; // If checkbox is checked, mark attendance as true
        //                }
        //                else
        //                {
        //                    attendee.IsAttend = false; // Otherwise, mark attendance as false
        //                }

        //                // Map the ViewModel back to the entity (assuming you need to update in the database)
        //                var eventAttendenceEntity = new tblEventAttendence
        //                {
        //                    Id = attendee.Id,
        //                    EventsId = attendee.EventsId,
        //                    EventsRegId = attendee.EventsRegId,
        //                    Day = attendee.Day,
        //                    IsAttend = attendee.IsAttend,
        //                    EventDate = attendee.EventDate
        //                };

        //                // Save the changes to the database
        //                _context.tblEventAttendence.Update(eventAttendenceEntity); // Use the entity model for updating
        //            }

        //            await _context.SaveChangesAsync(); // Commit changes to the database

        //            ViewData["Successful"] = "تم التحضير بنجاح"; // Attendance was successfully recorded
        //        }
        //        else
        //        {
        //            ViewData["Falied"] = "حدث خطأ"; // Something went wrong
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ"; // Handle the exception and return an error message
        //    }

        //    // After processing, return the same attendees to the view
        //    var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
        //    return View(attendees); // Return the correct model
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Members(IFormCollection forms, Guid id, int day)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var username = User.FindFirst(ClaimTypes.Name)?.Value;

        //            // Get all the attendance records for the given event and day
        //            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);

        //            // Get the list of attendee IDs that were checked in the form
        //            var attendanceStatusKeys = forms.Keys.Where(k => k.StartsWith("attendanceStatus["));
        //            var attendedIds = new List<Guid>();

        //            // Extract the IDs from the keys
        //            foreach (var key in attendanceStatusKeys)
        //            {
        //                // Extract the ID from the key (assuming key is in the format "attendanceStatus[GUID]")
        //                var idStr = key.Substring(17, key.Length - 18);
        //                if (Guid.TryParse(idStr, out Guid attendeeId))
        //                {
        //                    attendedIds.Add(attendeeId);
        //                }
        //            }

        //            foreach (var attendee in Ereg)
        //            {
        //                // Update IsAttend based on whether the attendee's checkbox was checked
        //                if (attendedIds.Contains(attendee.Id))
        //                {
        //                    attendee.IsAttend = true; // Mark as attended
        //                }
        //                else
        //                {
        //                    attendee.IsAttend = false; // Mark as not attended
        //                }

        //                // Map the ViewModel back to the entity for updating in the database
        //                var eventAttendenceEntity = new tblEventAttendence
        //                {
        //                    Id = attendee.Id,
        //                    EventsId = attendee.EventsId,
        //                    EventsRegId = attendee.EventsRegId,
        //                    Day = attendee.Day,
        //                    IsAttend = attendee.IsAttend,
        //                    EventDate = attendee.EventDate
        //                };

        //                // Save the changes to the database
        //                _context.tblEventAttendence.Update(eventAttendenceEntity);
        //            }

        //            await _context.SaveChangesAsync(); // Commit changes to the database

        //            ViewData["Successful"] = "تم التحضير بنجاح"; // Attendance was successfully recorded

        //            // After processing, return the updated attendees to the view
        //            return View(Ereg);
        //        }
        //        else
        //        {
        //            ViewData["Falied"] = "حدث خطأ"; // Something went wrong
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ"; // Handle the exception and return an error message
        //    }

        //    // If there's an error, reload the attendees
        //    var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
        //    return View(attendees); // Return the correct model
        //}




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        //public async Task<IActionResult> Members(IFormCollection forms, Guid id, int day)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // Get all attendees for this event and day
        //            var allAttendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);

        //            // Retrieve the IDs of attendees whose checkboxes were checked in the form
        //            var checkedAttendees = forms["attendanceStatus"].Select(x => Guid.Parse(x)).ToList();

        //            // Iterate through all attendees and set their IsAttend status
        //            foreach (var attendee in allAttendees)
        //            {
        //                bool isAttending = checkedAttendees.Contains(attendee.Id);
        //                _eventattendenceDomain.SetAttendanceStatus(attendee.Id, isAttending);
        //            }

        //            ViewData["Successful"] = "تم التحضير بنجاح";
        //        }
        //        else
        //        {
        //            ViewData["Falied"] = "حدث خطأ";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ";
        //    }

        //    var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
        //    return View(attendees); // Return the correct model
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> Members(FormCollection forms, Guid id, int day)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //var eventId = new Guid(forms["eventId"]);
        //            var username = User.FindFirst(ClaimTypes.Name)?.Value;

        //            // Find the specific attendance record by EventId and Day
        //            var attendance = await _context.tblEventAttendence
        //            .FirstOrDefaultAsync(a => a.EventsId == id && a.Day == day);

        //            if (attendance == null)
        //            {
        //                throw new InvalidOperationException("لايوجد اعضاء مسجلين");
        //            }

        //            // Perform the update (e.g., update the IsAttend field)
        //            attendance.IsAttend = true;

        //            // Save the changes
        //            _context.tblEventAttendence.Update(attendance);
        //            await _context.SaveChangesAsync();

        //            var count = forms["attendanceStatus"].Count;



        //            var Ereg = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);//ناخذ قائمة الفعاليات من الدومين ايفنت اتيندينس دومين
        //                                                                                        //return View(Ereg);

        //            if (count == 1)
        //                ViewData["Successful"] = "تم التحضير بنجاح";
        //            else
        //                ViewData["Falied"] = "حدث خطأ";

        //            return View(forms);

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ";
        //    }

        //    var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
        //    return View(attendees); // Return the correct model
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "NoRole,User,Admin,Super Admin,Editor")]
        //public async Task<IActionResult> Members(IFormCollection forms, Guid id, int day)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            string username = User.FindFirst(ClaimTypes.Name)?.Value;


        //            // Get all attendees for this event and day
        //            var allAttendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);

        //            // Retrieve the IDs of attendees whose checkboxes were checked in the form
        //            var checkedAttendees = forms.Keys
        //                .Where(k => k.StartsWith("attendanceStatus["))
        //                .Select(k => Guid.Parse(k.Split('[')[1].TrimEnd(']')))
        //                .ToList();

        //            // Iterate through all attendees and set their IsAttend status
        //            foreach (var attendee in allAttendees)
        //            {
        //                bool isAttending = checkedAttendees.Contains(attendee.Id);
        //                _eventattendenceDomain.SetAttendanceStatus(attendee.Id, isAttending);
        //            }

        //            ViewData["Successful"] = "تم التحضير بنجاح";
        //        }
        //        else
        //        {
        //            ViewData["Falied"] = "حدث خطأ";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Falied"] = "حدث خطأ";
        //    }

        //    var attendees = await _eventattendenceDomain.GetTblEventattendenceAsync(id, day);
        //    return View(attendees); // Return the correct model
        //}



    }
}
