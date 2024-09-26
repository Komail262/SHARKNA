using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class BoardTalRequestsDomain
    {
        private readonly SHARKNAContext _context;
        public BoardTalRequestsDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public IEnumerable<BoardTalRequestsViewModel> GetTblBoardTalRequests()
        {
            return _context.tblBoardTalRequests.Where(x => x.Board.IsDeleted == false)
                .Select(x => new BoardTalRequestsViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    BoardId = x.BoardId,
                    BoardName = x.Board.NameAr,
                    BoardDescription = x.Board.DescriptionAr,
                    RejectionReasons = x.RejectionReasons,
                    RequestStatusName = x.RequestStatus.RequestStatusAr,
                    RequestStatusId = x.RequestStatusId,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    MobileNumber = x.MobileNumber,
                    Skills = x.Skills,
                    Experiences = x.Experiences,


                }).ToList();
        }

        public BoardTalRequestsViewModel GetBoardTalRequestsById(Guid id)

        {
            return _context.tblBoardTalRequests
                .Where(x => x.Id == id && x.Board.IsDeleted == false)
                .Include(x => x.Board)
                .Include(x => x.RequestStatus)
                .Select(x => new BoardTalRequestsViewModel
                {
                    Id = id,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    UserName = x.UserName,
                    BoardName = x.Board.NameAr,
                    BoardDescription = x.Board.DescriptionAr,
                    RequestStatusId = x.RequestStatusId,
                    RequestStatusName = x.RequestStatus.RequestStatusAr,
                    RejectionReasons = x.RejectionReasons,
                    Email = x.Email,
                    MobileNumber = x.MobileNumber,
                    Skills = x.Skills,
                    Experiences = x.Experiences,

                })
                .FirstOrDefault();
        }

        public IEnumerable<BoardTalRequestsViewModel> GetTblBoardTalRequestsByUser(string username)
        {
            return _context.tblBoardTalRequests
                .Where(x => x.UserName == username)
                .Where(x => x.Board.IsDeleted == false)
                .Select(x => new BoardTalRequestsViewModel
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
                    MobileNumber = x.MobileNumber,
                    Skills = x.Skills,
                    Experiences = x.Experiences,
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
        public int AddUser(BoardTalRequestsViewModel BordTalReq, string UserName)
        {
            try
            {
                tblBoardTalRequests Vtal = new tblBoardTalRequests();
                Vtal.Id = BordTalReq.Id;
                Vtal.UserName = BordTalReq.UserName;
                Vtal.Email = BordTalReq.Email;
                Vtal.BoardId = BordTalReq.BoardId;
                Vtal.RequestStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
                Vtal.FullNameAr = BordTalReq.FullNameAr;
                Vtal.FullNameEn = BordTalReq.FullNameEn;
                Vtal.MobileNumber = BordTalReq.MobileNumber;
                Vtal.Skills = BordTalReq.Skills;
                Vtal.Experiences = BordTalReq.Experiences;
                Vtal.RejectionReasons = BordTalReq.RejectionReasons;


                _context.Add(Vtal);

                _context.SaveChanges();

                tblBoardTalRequestLogs bLogs = new tblBoardTalRequestLogs();
                bLogs.Id = Guid.NewGuid();
                bLogs.ReqId = Vtal.Id;
                bLogs.OpType = "اضافة";
                bLogs.OpDateTime = DateTime.Now;
                bLogs.CreatedBy = UserName;
                bLogs.AdditionalInfo = $"تم إضافة  {Vtal.FullNameAr}  الى  {Vtal.BoardId}";
                _context.tblBoardTalRequestLogs.Add(bLogs);

                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int AddBoardReqAdmin(BoardTalRequestsViewModel BordTalReq, string UserName)
        {

            try
            {
                tblBoardTalRequests Vtal = new tblBoardTalRequests();
                Vtal.Id = BordTalReq.Id;
                Vtal.UserName = BordTalReq.UserName;
                Vtal.BoardId = BordTalReq.BoardId;
                Vtal.RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152");
                Vtal.Email = BordTalReq.Email;
                Vtal.FullNameAr = BordTalReq.FullNameAr;
                Vtal.FullNameEn = BordTalReq.FullNameEn;
                Vtal.Skills = BordTalReq.Skills;
                Vtal.Experiences = BordTalReq.Experiences;
                Vtal.MobileNumber = BordTalReq.MobileNumber;

                _context.Add(BordTalReq);

                _context.SaveChanges();

                tblBoardTalRequestLogs bLogs = new tblBoardTalRequestLogs();
                bLogs.Id = Guid.NewGuid();
                bLogs.ReqId = BordTalReq.Id;
                bLogs.OpType = "اضافة";
                bLogs.OpDateTime = DateTime.Now;
                bLogs.CreatedBy = UserName;
                bLogs.AdditionalInfo = $"تم إضافة  {BordTalReq.FullNameAr}  الى  {BordTalReq.BoardId}";
                _context.tblBoardTalRequestLogs.Add(bLogs);

                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }



        public int CancelRequest(Guid id)
        {
            try
            {
                var Request = _context.tblBoardTalRequests.FirstOrDefault(r => r.Id == id);
                if (Request != null)
                {
                    _context.SaveChanges();

                    tblBoardTalRequestLogs bLogs = new tblBoardTalRequestLogs
                    {
                        Id = Guid.NewGuid(),
                        ReqId = Request.Id,
                        OpType = "تم الالغاء",
                        OpDateTime = DateTime.Now,
                        CreatedBy = Request.UserName,
                        AdditionalInfo = $"تم الغاء طلب {Request.FullNameAr} في {Request.Board.NameAr}"
                    };
                    _context.tblBoardTalRequestLogs.Add(bLogs);
                    _context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool IsEmailDuplicate(string email, Guid? userId = null)
        {
            if (userId == null)
            {
                return _context.tblBoardTalRequests.Any(u => u.Email == email);

            }
            else
            {
                return _context.tblBoardTalRequests.Any(u => u.Email == email && u.Id != userId);
            }
        }

        public List<tblBoards> GettblBoard()
        {
            return _context.tblBoards.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }



        public async Task Accepted(Guid id)
        {
            try
            {
                var BoardTalRequest = _context.tblBoardTalRequests.FirstOrDefault(r => r.Id == id);
                if (BoardTalRequest != null)
                {
                    BoardTalRequest.RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152"); // مقبول
                    await _context.SaveChangesAsync();

                    tblBoardTalRequestLogs bLogs = new tblBoardTalRequestLogs
                    {
                        Id = Guid.NewGuid(),
                        ReqId = BoardTalRequest.Id,
                        OpType = "Accept",
                        OpDateTime = DateTime.Now,
                        CreatedBy = BoardTalRequest.UserName,
                        AdditionalInfo = $"تم قبول طلب {BoardTalRequest.FullNameAr} في {BoardTalRequest.Board.NameAr}"
                    };
                    _context.tblBoardTalRequestLogs.Add(bLogs);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {

            }
        }

        public void Rejected(Guid id, string rejectionReason)
        {
            var BoardTalRequest = _context.tblBoardTalRequests.FirstOrDefault(r => r.Id == id);
            if (BoardTalRequest != null)
            {
                BoardTalRequest.RequestStatusId = Guid.Parse("271A02AD-8510-406C-BEB4-832BF79159D4"); // مرفوض
                BoardTalRequest.RejectionReasons = rejectionReason;
                _context.SaveChanges();
            }
        }

        public bool CheckRequestExists(string email, Guid boardId)
        {
            Guid Cancel = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
            Guid Accept = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152");

            return _context.tblBoardTalRequests
                .Any(r => r.Email == email && r.BoardId == boardId &&
                          (r.RequestStatusId == Cancel || r.RequestStatusId == Accept));
        }


    }
}