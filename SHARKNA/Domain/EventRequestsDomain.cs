using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class EventRequestsDomain
    {
        private readonly SHARKNAContext _context;

        public EventRequestsDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<EventRequestViewModel> GetTblEventRequests()
        {
            return _context.tblEventRequests
                .Select(view => new EventRequestViewModel
                {
                    Id = view.Id,
                    RejectionReasons = view.RejectionReasons,
                    EventId = view.EventId,
                    EventName = view.Event.EventTitleAr ?? "Unknown Event",
                    RequestStatusId = view.RequestStatusId,
                    RequestStatusName = view.RequestStatus.RequestStatusAr ?? "Unknown Status",
                    BoardId = view.BoardId,
                    BoardName = view.Board.NameAr ?? "Unknown Board"
                })
                .ToList();
        }

        public EventRequestViewModel GetEventRequestById(Guid id)
        {
            var request = _context.tblEventRequests
                .Include(r => r.Event)
                .Include(r => r.Board)
                .Include(r => r.RequestStatus)
                .FirstOrDefault(r => r.Id == id);

            if (request != null)
            {
                return new EventRequestViewModel
                {
                    Id = request.Id,
                    RejectionReasons = request.RejectionReasons,
                    EventId = request.EventId,
                    EventName = request.Event.EventTitleAr,
                    EventDescriptionAr = request.Event.DescriptionAr, // وصف الفعالية
                    TopicAr = request.Event.TopicAr,
                    EventStartDate = request.Event.EventStartDate,
                    EventEndtDate = request.Event.EventEndtDate,
                    LocationAr = request.Event.LocationAr,
                    MaxAttendence = request.Event.MaxAttendence,
                    SpeakersAr = request.Event.SpeakersAr,
                    RequestStatusId = request.RequestStatusId,
                    RequestStatusName = request.RequestStatus.RequestStatusAr,
                    Gender = request.Event.Gender,
                    BoardId = request.BoardId,
                    BoardName = request.Board.NameAr,
                    BoardDescriptionAr = request.Board.DescriptionAr // وصف اللجنة
                };
            }
            return null;
        }


        public void AddEventViewRequest(EventRequestViewModel view)
        {
            var newRequest = new tblEventRequests
            {
                Id = view.Id,
                RejectionReasons = view.RejectionReasons,
                EventId = view.EventId,
                RequestStatusId = view.RequestStatusId,
                BoardId = view.BoardId
            };

            _context.tblEventRequests.Add(newRequest);
            _context.SaveChanges();
        }

        public void CancelEventRequest(Guid id)
        {
            var request = _context.tblEventRequests.FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                // تأكد من استخدام الـ ID الصحيح لحالة "تم الإلغاء"
                var canceledStatus = _context.tblRequestStatus.FirstOrDefault(s => s.Id == Guid.Parse("11E42297-D061-42A0-B190-7D7B26936BAB"));
                if (canceledStatus != null)
                {
                    request.RequestStatusId = canceledStatus.Id;
                    _context.SaveChanges(); // تأكد من حفظ التغييرات في قاعدة البيانات
                }
            }
        }


        public List<tblBoards> GetTblBoards()
        {
            return _context.tblBoards.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }

        public List<tblEvents> GetTblEventDomain()
        {
            return _context.tblEvents.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }
        public void UpdateEventRequestBoard(Guid eventId, Guid newBoardId)
        {
            // جلب جميع الطلبات المرتبطة بالحدث
            var eventRequests = _context.tblEventRequests.Where(r => r.EventId == eventId).ToList();

            // تحديث BoardId في كل طلب
            foreach (var request in eventRequests)
            {
                request.BoardId = newBoardId;
            }

            // حفظ التغييرات في قاعدة البيانات
            _context.SaveChanges();
        }


        public async Task AcceptRequest(Guid requestId)
        {
            var request = _context.tblEventRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                var acceptedStatus = _context.tblRequestStatus.FirstOrDefault(s => s.RequestStatusAr == "مقبول");
                if (acceptedStatus != null)
                {
                    request.RequestStatusId = acceptedStatus.Id;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public void RejectRequest(Guid requestId, string rejectionReason)
        {
            var request = _context.tblEventRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                var rejectedStatus = _context.tblRequestStatus.FirstOrDefault(s => s.RequestStatusAr == "مرفوض");
                if (rejectedStatus != null)
                {
                    request.RequestStatusId = rejectedStatus.Id;
                    request.RejectionReasons = rejectionReason;
                    _context.SaveChanges();
                }
            }
        }


        public List<tblRequestStatus> GetTblRequestStatus()
        {
            return _context.tblRequestStatus.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }

        public int UpdateRequestStatus(Guid requestId, Guid requestStatusId, string rejectionReasons)
        {
            var request = _context.tblEventRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.RequestStatusId = requestStatusId; // تحديث حالة الطلب

                // إذا كانت الحالة "مرفوض"، قم بتحديث أسباب الرفض
                var selectedStatus = _context.tblRequestStatus.FirstOrDefault(rs => rs.Id == requestStatusId)?.RequestStatusAr;
                if (selectedStatus == "مرفوض")
                {
                    request.RejectionReasons = rejectionReasons;
                }
                else
                {
                    request.RejectionReasons = null;
                }

                _context.SaveChanges(); // حفظ التغييرات في قاعدة البيانات
                return 1; // ناجح
            }
            return 0; // فشل
        }

    }
}




