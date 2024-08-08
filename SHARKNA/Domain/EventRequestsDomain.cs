using System.Collections.Generic;
using System.Linq;
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
            return _context.tblEventRequests.Select(view => new EventRequestViewModel
            {
                Id = view.Id,
                RejectionReasons = view.RejectionReasons,
                EventId = view.EventId,
                EventName = view.Event.EventTitleAr,
                RequestStatusId = view.RequestStatusId,
                RequestStatusName = view.RequestStatus.RequestStatusAr,
                BoardId = view.BoardId,
                BoardName = view.Board.NameAr
            }).ToList();
        }

        public void AddEventViewRequest(EventRequestViewModel view)
        {
            tblEventRequests TableForEventRequest = new tblEventRequests
            {
                Id = view.Id,
                RejectionReasons = view.RejectionReasons,
                EventId = view.EventId,
                RequestStatusId = view.RequestStatusId, // استخدم الحالة المحددة من المستخدم
                BoardId = view.BoardId
            };

            _context.tblEventRequests.Add(TableForEventRequest);
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
    }
}
