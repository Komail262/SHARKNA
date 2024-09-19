using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHARKNA.Domain
{
    public class BoardRequestsDomain
    {
        private readonly SHARKNAContext _context;
        public BoardRequestsDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BoardRequestsViewModel>> GetTblBoardRequestsAsync()
        {
            return await _context.tblBoardRequests
                .Where(x => x.Board.IsDeleted == false)
                .Select(x => new BoardRequestsViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    BoardId = x.BoardId,
                    BoardName = x.Board.NameAr,
                    BoardDescription = x.Board.DescriptionAr,
                    RejectionReasons = x.RejectionReasons,
                    RequestStatusName = x.RequestStatus.RequestStatusAr,
                    RequestStatusId = x.RequestStatusId,
                    Email = x.Email,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    MobileNumber = x.MobileNumber
                }).ToListAsync();
        }

        public async Task<BoardRequestsViewModel> GetBoardRequestByIdAsync(Guid id)
        {
            return await _context.tblBoardRequests
                .Where(x => x.Id == id && x.Board.IsDeleted == false)
                .Include(x => x.Board)
                .Include(x => x.RequestStatus)
                .Select(x => new BoardRequestsViewModel
                {
                    Id = x.Id,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    UserName = x.UserName,
                    Email = x.Email,
                    MobileNumber = x.MobileNumber,
                    BoardName = x.Board.NameAr,
                    BoardDescription = x.Board.DescriptionAr,
                    RequestStatusId = x.RequestStatusId,
                    RequestStatusName = x.RequestStatus.RequestStatusAr,
                    RejectionReasons = x.RejectionReasons
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BoardRequestsViewModel>> GetTblBoardRequestsByUserAsync(string username)
        {
            return await _context.tblBoardRequests
                .Where(x => x.UserName == username)
                .Where(x => x.Board.IsDeleted == false)
                .Select(x => new BoardRequestsViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    BoardId = x.BoardId,
                    BoardName = x.Board.NameAr,
                    BoardDescription = x.Board.DescriptionAr,
                    RejectionReasons = x.RejectionReasons,
                    RequestStatusName = x.RequestStatus.RequestStatusAr,
                    RequestStatusId = x.RequestStatusId,
                    Email = x.Email,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    MobileNumber = x.MobileNumber
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<tblUsers>> GetAllUsers()
        {
            try
            {
                return await _context.tblUsers.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<tblUsers>();
            }
        }

        public async Task<int> AddBoardReqAsync(BoardRequestsViewModel BoardReq, string UserName , string username)
        {
            try
            {
                tblBoardRequests VBoardReq = new tblBoardRequests
                {
                    Id = BoardReq.Id,
                    UserName = BoardReq.UserName,
                    BoardId = BoardReq.BoardId,
                    RequestStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2"),
                    Email = BoardReq.Email,
                    FullNameAr = BoardReq.FullNameAr,
                    FullNameEn = BoardReq.FullNameEn,
                    MobileNumber = BoardReq.MobileNumber
                };

                await _context.AddAsync(VBoardReq);
                await _context.SaveChangesAsync();

                tblBoardRequestLogs BR = new tblBoardRequestLogs();
                BR.Id = Guid.NewGuid();
                BR.ReqId = VBoardReq.Id;
                BR.OpType = "اضافة";
                BR.OpDateTime = DateTime.Now;
                BR.CreatedBy = UserName;
                BR.AdditionalInfo = $"تم إضافة  {VBoardReq.FullNameAr} بواسطة هذا المستخدم {username}";

                await _context.tblBoardRequestLogs.AddAsync(BR);
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddBoardReqAdminAsync(BoardRequestsViewModel BoardReq)
        {
            try
            {
                tblBoardRequests VBoardReq = new tblBoardRequests
                {
                    Id = BoardReq.Id,
                    UserName = BoardReq.UserName,
                    BoardId = BoardReq.BoardId,
                    RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152"),
                    Email = BoardReq.Email,
                    FullNameAr = BoardReq.FullNameAr,
                    FullNameEn = BoardReq.FullNameEn,
                    MobileNumber = BoardReq.MobileNumber
                };

                await _context.AddAsync(VBoardReq);
                await _context.SaveChangesAsync();

                tblBoardRequestLogs BR = new tblBoardRequestLogs();
                BR.Id = Guid.NewGuid();
                BR.ReqId = VBoardReq.Id;
                BR.OpType = "اضافة";
                BR.OpDateTime = DateTime.Now;
                BR.CreatedBy = VBoardReq.UserName;
                BR.AdditionalInfo = $"تم إضافة  {VBoardReq.FullNameAr} بواسطة هذا المستخدم ";

                await _context.tblBoardRequestLogs.AddAsync(BR);
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> IsEmailDuplicateAsync(string email, Guid? BoardReqId = null)
        {
            if (BoardReqId == null)
            {
                return await _context.tblBoardRequests.AnyAsync(u => u.Email == email);
            }
            else
            {
                return await _context.tblBoardRequests.AnyAsync(u => u.Email == email && u.Id != BoardReqId);
            }
        }

        public async Task<int> CancelRequestAsync(Guid id, string username)
        {
            try
            {
                var BoardRequest = await _context.tblBoardRequests.FirstOrDefaultAsync(r => r.Id == id);

                if (BoardRequest != null)
                {
                    BoardRequest.RequestStatusId = Guid.Parse("11E42297-D061-42A0-B190-7D7B26936BAB"); // تعيين الحالة "تم الإلغاء"
                    await _context.SaveChangesAsync();

                    tblBoardRequestLogs BR = new tblBoardRequestLogs();
                    BR.Id = Guid.NewGuid();
                    BR.ReqId = BoardRequest.Id;
                    BR.OpType = "تم الالغاء";
                    BR.OpDateTime = DateTime.Now;
                    BR.CreatedBy = BoardRequest.UserName;
                    BR.AdditionalInfo = $"تم الغاء طلب {BoardRequest.FullNameAr} بواسطة هذا المستخدم {username}";

                    await _context.tblBoardRequestLogs.AddAsync(BR);
                    await _context.SaveChangesAsync();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task AcceptAsync(Guid id , string username)
        {
            try
            {
                var BoardRequest = await _context.tblBoardRequests.FirstOrDefaultAsync(r => r.Id == id);
                if (BoardRequest != null)
                {
                    BoardRequest.RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152"); // تعيين الحالة "مقبول"

                    await _context.SaveChangesAsync();

                    tblBoardMembers member = new tblBoardMembers
                    {
                        BoardId = BoardRequest.BoardId,
                        BoardRoleId = Guid.Parse("7d67185d-81bd-4738-a6c5-2106e441eea1"),
                        Email = BoardRequest.Email,
                        FullNameAr = BoardRequest.FullNameAr,
                        FullNameEn = BoardRequest.FullNameEn,
                        IsDeleted = false,
                        IsActive = true,
                        MobileNumber = BoardRequest.MobileNumber,
                        UserName = BoardRequest.UserName,
                    };
                    await _context.AddAsync(member);
                    await _context.SaveChangesAsync();

                    tblBoardRequestLogs BR = new tblBoardRequestLogs();
                    BR.Id = Guid.NewGuid();
                    BR.ReqId = BoardRequest.Id;
                    BR.OpType = "تم القبول";
                    BR.OpDateTime = DateTime.Now;
                    BR.CreatedBy = BoardRequest.UserName;
                    BR.AdditionalInfo = $"تم قبول الطلب {BoardRequest.FullNameAr} بواسطة هذا المستخدم {username}";

                    await _context.tblBoardRequestLogs.AddAsync(BR);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        public async Task RejectAsync(Guid id, string rejectionReason , string username)
        {
            var BoardRequest = await _context.tblBoardRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (BoardRequest != null)
            {
                BoardRequest.RequestStatusId = Guid.Parse("271A02AD-8510-406C-BEB4-832BF79159D4"); // تعيين الحالة "مرفوض"
                BoardRequest.RejectionReasons = rejectionReason;
                await _context.SaveChangesAsync();

                tblBoardRequestLogs BR = new tblBoardRequestLogs();
                BR.Id = Guid.NewGuid();
                BR.ReqId = BoardRequest.Id;
                BR.OpType = "تم الرفض";
                BR.OpDateTime = DateTime.Now;
                BR.CreatedBy = BoardRequest.UserName;
                BR.AdditionalInfo = $"تم رفض الطلب {BoardRequest.FullNameAr} بواسطة هذا المستخدم {username} بسبب: {rejectionReason}";

                await _context.tblBoardRequestLogs.AddAsync(BR);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<bool> CanUserMakeNewRequestAsync(string email, Guid boardId)
        {
           
            Guid UnderStudy = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
            Guid Accept = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152");

            bool hasUnderStudyRequest = await _context.tblBoardRequests
                .AnyAsync(r => r.Email == email && r.BoardId == boardId && r.RequestStatusId == UnderStudy);

            if (hasUnderStudyRequest)
            {
                return false;
            }

            bool isCurrentMember = await _context.tblBoardMembers
                .AnyAsync(m => m.Email == email && m.BoardId == boardId && m.IsDeleted == false);

            if (isCurrentMember)
            {
                return false;
            }

            bool wasAcceptedAndDeleted = await _context.tblBoardMembers
                .AnyAsync(m => m.Email == email && m.BoardId == boardId && m.IsDeleted == true);

            if (wasAcceptedAndDeleted)
            {
                return true;
            }

            bool hasAcceptedRequest = await _context.tblBoardRequests
                .AnyAsync(r => r.Email == email && r.BoardId == boardId && r.RequestStatusId == Accept);

            if (hasAcceptedRequest)
            {
                return false;
            }

            return true;
        }

    }
}
