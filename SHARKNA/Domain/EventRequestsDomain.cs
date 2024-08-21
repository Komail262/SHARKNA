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
                    Time = request.Event.Time,
                    EndRegTime = request.Event.EndRegTime,
                    LocationAr = request.Event.LocationAr,
                    MaxAttendence = request.Event.MaxAttendence,
                    SpeakersAr = request.Event.SpeakersAr,
                    RequestStatusId = request.RequestStatusId,
                    RequestStatusName = request.RequestStatus.RequestStatusAr,
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
                var canceledStatus = _context.tblRequestStatus.FirstOrDefault(s => s.Id == Guid.Parse("E2D4F6A8-B2C3-4E5D-8A9F-0B1C2D3E4F5A"));
                if (canceledStatus != null)
                {
                    request.RequestStatusId = canceledStatus.Id;
                    _context.SaveChanges();
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

        public List<tblRequestStatus> GetTblRequestStatus()
        {
            return _context.tblRequestStatus.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }

        public int UpdateRequestStatus(Guid requestId, Guid requestStatusId, string rejectionReasons)
        {
            var request = _context.tblEventRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.RequestStatusId = requestStatusId;

                var selectedStatus = _context.tblRequestStatus.FirstOrDefault(rs => rs.Id == requestStatusId)?.RequestStatusAr;

                if (selectedStatus == "مرفوض")
                {
                    request.RejectionReasons = rejectionReasons;
                }
                else
                {
                    request.RejectionReasons = null;
                }

                _context.SaveChanges();
                return 1; // ناجح
            }
            return 0; // فشل
        }
    }
}
