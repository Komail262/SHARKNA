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



        public async Task<List<EventRequestViewModel>> GetEventRequestsByUserAsync(string username)
        {
            var requests = await (from request in _context.tblEventRequests
                                  join log in _context.tblEventRequestLogs on request.Id equals log.ReqId
                                  where log.CreatedBy == username // جلب الطلبات الخاصة بالمستخدم الحالي فقط
                                  group new { request, log } by request.Id into groupedRequests // نقوم بتجميع الطلبات لمنع التكرار
                                  select new EventRequestViewModel
                                  {
                                      Id = groupedRequests.Key,
                                      EventName = groupedRequests.FirstOrDefault().request.Event.EventTitleAr,
                                      BoardName = groupedRequests.FirstOrDefault().request.Board.NameAr,
                                      RequestStatusName = groupedRequests.FirstOrDefault().request.RequestStatus.RequestStatusAr,
                                      RequestStatusId = groupedRequests.FirstOrDefault().request.RequestStatusId,
                                      RejectionReasons = groupedRequests.FirstOrDefault().request.RejectionReasons,
                                      EventId = groupedRequests.FirstOrDefault().request.EventId
                                  }).ToListAsync();

            return requests;
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
                // جلب معلومات المستخدم الذي أنشأ الطلب من جدول tblEventRequestLogs
                var userLog = await _context.tblEventRequestLogs
                    .Where(l => l.ReqId == id && l.OpType == "إضافة")
                    .OrderByDescending(l => l.OpDateTime)
                    .FirstOrDefaultAsync();

                var user = await _context.tblUsers.FirstOrDefaultAsync(u => u.UserName == userLog.CreatedBy);

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
                    BoardDescriptionAr = request.Board.DescriptionAr,

                    // إضافة معلومات المستخدم
                    CreatedByUserName = user.UserName,
                    CreatedByFullName = user.FullNameAr,
                    CreatedByEmail = user.Email,
                    CreatedByMobileNumber = user.MobileNumber,
                    CreatedByGender = user.Gender == true ? "ذكر" : "أنثى" // Assuming Gender is a boolean in tblUsers
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

        //public async Task<int> UpdateRequestStatusAsync(Guid requestId, Guid requestStatusId, string rejectionReasons, string username)
        //{
        //    var request = await _context.tblEventRequests.FirstOrDefaultAsync(r => r.Id == requestId);
        //    if (request != null)
        //    {
        //        request.RequestStatusId = requestStatusId;
        //        var selectedStatus = await _context.tblRequestStatus.FirstOrDefaultAsync(rs => rs.Id == requestStatusId);

        //        if (selectedStatus?.RequestStatusAr == "مرفوض")
        //        {
        //            request.RejectionReasons = rejectionReasons;
        //        }
        //        else
        //        {
        //            request.RejectionReasons = null;
        //        }

        //        await _context.SaveChangesAsync();

        //        tblEventRequestLogs logs = new tblEventRequestLogs();
        //        logs.Id = Guid.NewGuid();
        //        logs.ReqId = request.Id;
        //        logs.OpType = "تحديث الحالة";
        //        logs.OpDateTime = DateTime.Now;
        //        logs.CreatedBy = username;
        //        logs.AdditionalInfo = $"تم تحديث حالة الطلب {request.Id} إلى {selectedStatus?.RequestStatusAr} بواسطة هذا المستخدم {username}";
        //        _context.tblEventRequestLogs.Add(logs);
        //        await _context.SaveChangesAsync();

        //        return 1;
        //    }
        //    return 0;
        //}

        public async Task<List<tblBoards>> GetTblBoardsAsync()
        {
            return await _context.tblBoards.Where(e => !e.IsDeleted && e.IsActive).ToListAsync();
        }

        //public async Task<List<tblBoards>> GetTblBoardsByUserAsync(string username)
        //{
        //    // ابحث عن جميع BoardId التي يتبع لها الـ username في جدول tblBoardMembers حيث تكون IsDeleted = false و IsActive = true
        //    var userBoardIds = await _context.tblBoardMembers
        //        .Where(bm => bm.UserName == username && bm.IsActive && !bm.IsDeleted)
        //        .Select(bm => bm.BoardId)
        //        .ToListAsync();

        //    // جلب الـ Boards التي تتعلق بـ BoardId من المستخدم حيث تكون IsDeleted = false و IsActive = true
        //    var boards = await _context.tblBoards
        //        .Where(b => userBoardIds.Contains(b.Id) && b.IsActive && !b.IsDeleted)
        //        .ToListAsync();

        //    return boards;
        //}

        public async Task<List<tblBoards>> GetTblBoardsByUserAsync(string username)
        {
            // جلب الـ Boards مع BoardMembers المتعلقين بـ username المحدد
            return await _context.tblBoards
                .Include(b => b.BoardMembers) // جلب بيانات BoardMembers المرتبطة
                .Where(b => !b.IsDeleted && b.IsActive &&
                            b.BoardMembers.Any(bm => !bm.IsDeleted && bm.IsActive && bm.UserName == username &&
                                (bm.BoardRoleId == Guid.Parse("4F573067-8291-472C-ABEC-E6C1763A2BA1") || bm.BoardRoleId == Guid.Parse("D86E15D5-FF79-4757-89BC-F876A0A72B75")))) // التحقق من IsDeleted و IsActive لكل من Board و BoardMembers بالإضافة إلى شرط Username و BoardRoleId
                .ToListAsync();
        }



        //public async Task<List<tblBoards>> GetTblBoardsAsync()
        //{
        //    // استخدام Include لجلب BoardMembers المرتبطة مع كل Board
        //    return await _context.tblBoards
        //        .Include(b => b.BoardMembers) // جلب بيانات BoardMembers المرتبطة
        //        .Where(e => !e.IsDeleted && e.IsActive && e.BoardMembers.Any(bm => !bm.IsDeleted && bm.IsActive)) // التحقق من IsDeleted و IsActive لكل من Board و BoardMembers
        //        .ToListAsync();
        //}



    }
}
