using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Threading.Tasks;

namespace SHARKNA.Domain
{
    public class EventRequestsDomain
    {
        private readonly SHARKNAContext _context;
        private readonly UserDomain _userDomain;

        public EventRequestsDomain(SHARKNAContext context, UserDomain userDomain)
        {
            _context = context;
            _userDomain = userDomain;
        }

        public async Task<IEnumerable<EventRequestViewModel>> GetTblEventRequestsAsync()
        {
            return await _context.tblEventRequests
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
                .ToListAsync();
        }

        public async Task<EventRequestViewModel> GetEventRequestByIdAsync(Guid id)
        {
            var request = await _context.tblEventRequests
                .Include(r => r.Event)
                .Include(r => r.Board)
                .Include(r => r.RequestStatus)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request != null)
            {
                return new EventRequestViewModel
                {
                    Id = request.Id,
                    RejectionReasons = request.RejectionReasons,
                    EventId = request.EventId,
                    EventName = request.Event.EventTitleAr,
                    EventDescriptionAr = request.Event.DescriptionAr,
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
                    BoardDescriptionAr = request.Board.DescriptionAr
                };
            }
            return null;
        }

        public async Task AddEventViewRequestAsync(EventRequestViewModel view, string username)
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
            await _context.SaveChangesAsync();

            tblEventRequestLogs logs = new tblEventRequestLogs();
            logs.Id = Guid.NewGuid();
            logs.ReqId = newRequest.Id;
            logs.OpType = "إضافة";
            logs.OpDateTime = DateTime.Now;
            logs.CreatedBy = username;
            logs.AdditionalInfo = $"تم إضافة حدث {newRequest.Event} بواسطة هذا المستخدم {username}";
            _context.tblEventRequestLogs.Add(logs);
            await _context.SaveChangesAsync();
        }

        public async Task CancelEventRequestAsync(Guid id, string username)
        {
            var request = await _context.tblEventRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request != null)
            {
                var canceledStatus = await _context.tblRequestStatus.FirstOrDefaultAsync(s => s.Id == Guid.Parse("11E42297-D061-42A0-B190-7D7B26936BAB"));
                if (canceledStatus != null)
                {
                    request.RequestStatusId = canceledStatus.Id;
                    await _context.SaveChangesAsync();

                    tblEventRequestLogs logs = new tblEventRequestLogs();
                    logs.Id = Guid.NewGuid();
                    logs.ReqId = request.Id;
                    logs.OpType = "إلغاء";
                    logs.OpDateTime = DateTime.Now;
                    logs.CreatedBy = username;
                    logs.AdditionalInfo = $"تم إلغاء الحدث {request.EventId} بواسطة هذا المستخدم {username}";
                    _context.tblEventRequestLogs.Add(logs);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task AcceptRequestAsync(Guid requestId, string username)
        {
            var request = await _context.tblEventRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request != null)
            {
                var acceptedStatus = await _context.tblRequestStatus.FirstOrDefaultAsync(s => s.RequestStatusAr == "مقبول");
                if (acceptedStatus != null)
                {
                    request.RequestStatusId = acceptedStatus.Id;
                    await _context.SaveChangesAsync();

                    tblEventRequestLogs logs = new tblEventRequestLogs();
                    logs.Id = Guid.NewGuid();
                    logs.ReqId = request.Id;
                    logs.OpType = "قبول";
                    logs.OpDateTime = DateTime.Now;
                    logs.CreatedBy = username;
                    logs.AdditionalInfo = $"تم قبول الطلب {request.Id} بواسطة هذا المستخدم {username}";
                    _context.tblEventRequestLogs.Add(logs);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RejectRequestAsync(Guid requestId, string rejectionReason, string username)
        {
            var request = await _context.tblEventRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request != null)
            {
                var rejectedStatus = await _context.tblRequestStatus.FirstOrDefaultAsync(s => s.RequestStatusAr == "مرفوض");
                if (rejectedStatus != null)
                {
                    request.RequestStatusId = rejectedStatus.Id;
                    request.RejectionReasons = rejectionReason;
                    await _context.SaveChangesAsync();

                    tblEventRequestLogs logs = new tblEventRequestLogs();
                    logs.Id = Guid.NewGuid();
                    logs.ReqId = request.Id;
                    logs.OpType = "رفض";
                    logs.OpDateTime = DateTime.Now;
                    logs.CreatedBy = username;
                    logs.AdditionalInfo = $"تم رفض الطلب {request.Id} بواسطة هذا المستخدم {username} لسبب: {rejectionReason}";
                    _context.tblEventRequestLogs.Add(logs);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<tblRequestStatus>> GetTblRequestStatusAsync()
        {
            return await _context.tblRequestStatus.Where(e => !e.IsDeleted && e.IsActive).ToListAsync();
        }

        public async Task<int> UpdateRequestStatusAsync(Guid requestId, Guid requestStatusId, string rejectionReasons, string username)
        {
            var request = await _context.tblEventRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request != null)
            {
                request.RequestStatusId = requestStatusId;
                var selectedStatus = await _context.tblRequestStatus.FirstOrDefaultAsync(rs => rs.Id == requestStatusId);

                if (selectedStatus?.RequestStatusAr == "مرفوض")
                {
                    request.RejectionReasons = rejectionReasons;
                }
                else
                {
                    request.RejectionReasons = null;
                }

                await _context.SaveChangesAsync();

                tblEventRequestLogs logs = new tblEventRequestLogs();
                logs.Id = Guid.NewGuid();
                logs.ReqId = request.Id;
                logs.OpType = "تحديث الحالة";
                logs.OpDateTime = DateTime.Now;
                logs.CreatedBy = username;
                logs.AdditionalInfo = $"تم تحديث حالة الطلب {request.Id} إلى {selectedStatus?.RequestStatusAr} بواسطة هذا المستخدم {username}";
                _context.tblEventRequestLogs.Add(logs);
                await _context.SaveChangesAsync();

                return 1;
            }
            return 0;
        }

        public async Task<List<tblBoards>> GetTblBoardsAsync()
        {
            return await _context.tblBoards.Where(e => !e.IsDeleted && e.IsActive).ToListAsync();
        }
    }
}
