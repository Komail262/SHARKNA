using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SHARKNA.Domain
{
    public class BoardRequestsDomain
    {
        private readonly SHARKNAContext _context;
        public BoardRequestsDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<BoardRequestsViewModel> GetTblBoardRequests()
        {
            return _context.tblBoardRequests
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
            }).ToList();
        }



        public BoardRequestsViewModel GetBoardRequestById(Guid id)
        {
            return _context.tblBoardRequests
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
                .FirstOrDefault();
        }

        public IEnumerable<BoardRequestsViewModel> GetTblBoardRequestsByUser(string username)
        {
            return _context.tblBoardRequests
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
                .ToList();
        }

        public async Task<UserViewModel> GetTblUsersByUserName(string userName)
        {
            var user = await _context.tblUsers.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }

            return new UserViewModel
            {
                UserName = user.UserName,
                FullNameAr = user.FullNameAr,
                FullNameEn = user.FullNameEn,
                MobileNumber = user.MobileNumber,
                Email = user.Email
            };
        }

        public int AddBoardReq(BoardRequestsViewModel BoardReq, string UserName)
        {
           
            try
            {
                tblBoardRequests VBoardReq = new tblBoardRequests();
                VBoardReq.Id = BoardReq.Id;
                VBoardReq.UserName = BoardReq.UserName;
                VBoardReq.BoardId = BoardReq.BoardId;
                VBoardReq.RequestStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
                VBoardReq.Email = BoardReq.Email;
                VBoardReq.FullNameAr = BoardReq.FullNameAr;
                VBoardReq.FullNameEn = BoardReq.FullNameEn;
                VBoardReq.MobileNumber = BoardReq.MobileNumber;

                _context.Add(VBoardReq);

                _context.SaveChanges();

                tblBoardRequestLogs bLogs = new tblBoardRequestLogs();
                bLogs.Id = Guid.NewGuid();
                bLogs.ReqId = VBoardReq.Id;
                bLogs.OpType = "اضافة";
                bLogs.OpDateTime = DateTime.Now;
                bLogs.CreatedBy = UserName;
                bLogs.AdditionalInfo = $"تم إضافة  {VBoardReq.FullNameAr}  الى  {VBoardReq.BoardId}";
                _context.tblBoardRequestLogs.Add(bLogs);

                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int AddBoardReqAdmin(BoardRequestsViewModel BoardReq, string UserName)
        {

            try
            {
                tblBoardRequests VBoardReq = new tblBoardRequests();
                VBoardReq.Id = BoardReq.Id;
                VBoardReq.UserName = BoardReq.UserName;
                VBoardReq.BoardId = BoardReq.BoardId;
                VBoardReq.RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152");
                VBoardReq.Email = BoardReq.Email;
                VBoardReq.FullNameAr = BoardReq.FullNameAr;
                VBoardReq.FullNameEn = BoardReq.FullNameEn;
                VBoardReq.MobileNumber = BoardReq.MobileNumber;

                _context.Add(VBoardReq);

                _context.SaveChanges();

                tblBoardRequestLogs bLogs = new tblBoardRequestLogs();
                bLogs.Id = Guid.NewGuid();
                bLogs.ReqId = VBoardReq.Id;
                bLogs.OpType = "اضافة";
                bLogs.OpDateTime = DateTime.Now;
                bLogs.CreatedBy = UserName;
                bLogs.AdditionalInfo = $"تم إضافة  {VBoardReq.FullNameAr}  الى  {VBoardReq.BoardId}";
                _context.tblBoardRequestLogs.Add(bLogs);

                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }




        public bool IsEmailDuplicate(string email, Guid? BoardReqId = null)
        {
            if (BoardReqId == null)
            {
                return _context.tblBoardRequests.Any(u => u.Email == email);

            }
            else
            {
                return _context.tblBoardRequests.Any(u => u.Email == email && u.Id != BoardReqId);
            }
        }



        public int CancelRequest(Guid id)
        {
            try
            {

                var BoardRequest = _context.tblBoardRequests.FirstOrDefault(r => r.Id == id);

                if (BoardRequest != null)
                {
                    BoardRequest.RequestStatusId = Guid.Parse("11E42297-D061-42A0-B190-7D7B26936BAB"); // تعيين الحالة "تم الإلغاء"
                    _context.SaveChanges();

                    tblBoardRequestLogs bLogs = new tblBoardRequestLogs
                    {
                        Id = Guid.NewGuid(), 
                        ReqId = BoardRequest.Id,
                        OpType = "تم الالغاء",
                        OpDateTime = DateTime.Now,
                        CreatedBy = BoardRequest.UserName,
                        AdditionalInfo = $"تم الغاء طلب {BoardRequest.FullNameAr} في {BoardRequest.Board.NameAr}"
                    };
                    _context.tblBoardRequestLogs.Add(bLogs);
                    _context.SaveChanges();

                    
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task Accept(Guid id)
        {
            try
            {
                var BoardRequest = _context.tblBoardRequests.FirstOrDefault(r => r.Id == id);
                if (BoardRequest != null)
                {
                    BoardRequest.RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152"); // تعيين الحالة "مقبول"

                    await _context.SaveChangesAsync();
                    tblBoardMembers member = new tblBoardMembers()
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

                    tblBoardRequestLogs bLogs = new tblBoardRequestLogs
                    {
                        Id = Guid.NewGuid(),
                        ReqId = BoardRequest.Id,
                        OpType = "Accept",
                        OpDateTime = DateTime.Now,
                        CreatedBy = BoardRequest.UserName,
                        AdditionalInfo = $"تم قبول طلب {BoardRequest.FullNameAr} في {BoardRequest.Board.NameAr}"
                    };
                    _context.tblBoardRequestLogs.Add(bLogs);
                    _context.SaveChanges();
                }
               
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void Reject(Guid id, string rejectionReason)
        {
            var BoardRequest = _context.tblBoardRequests.FirstOrDefault(r => r.Id == id);
            if (BoardRequest != null)
            {
                BoardRequest.RequestStatusId = Guid.Parse("271A02AD-8510-406C-BEB4-832BF79159D4"); // تعيين الحالة "مرفوض"
                BoardRequest.RejectionReasons = rejectionReason;
                _context.SaveChanges();
            }
        }

        public bool CheckRequestExists(string email, Guid boardId)
        {
            Guid Cancel = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
            Guid Accept = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152");

            return _context.tblBoardRequests
                .Any(r => r.Email == email && r.BoardId == boardId &&
                          (r.RequestStatusId == Cancel || r.RequestStatusId == Accept));
        }


    }
}
