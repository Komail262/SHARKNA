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
                RequestStatusId = view.RequestStatusId,
                BoardId = view.BoardId
            };

            _context.tblEventRequests.Add(TableForEventRequest);
            _context.SaveChanges();
        }

        // الدالة الجديدة لإلغاء الطلب
        public void CancelRequest(Guid id)
        {
            var eventRequest = _context.tblEventRequests.FirstOrDefault(r => r.Id == id);
            if (eventRequest != null)
            {
                eventRequest.RequestStatusId = Guid.Parse("11E42297-D061-42A0-B190-7D7B26936BAB"); // تعيين الحالة "تم الإلغاء"
                _context.SaveChanges();
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
