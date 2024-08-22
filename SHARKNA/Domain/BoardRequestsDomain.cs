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
            return _context.tblBoardRequests.Select(x => new BoardRequestsViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                BoardId = x.BoardId,
                BoardName = x.Board.NameAr,
                RejectionReasons = x.RejectionReasons,
                RequestStatusName = x.RequestStatus.RequestStatusAr,
                RequestStatusId = x.RequestStatusId,
                Email = x.Email,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                MobileNumber = x.MobileNumber
            }).ToList();
        }

        public BoardRequestsViewModel GetTblBoardRequestsById(Guid id)
        {
            var BoardReq = _context.tblBoardRequests.FirstOrDefault(u => u.Id == id);
            BoardRequestsViewModel uu = new BoardRequestsViewModel();
            uu.Id = id;
            uu.UserName = BoardReq.UserName;
            uu.BoardId = BoardReq.BoardId;
            uu.RequestStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
            uu.RejectionReasons = BoardReq.RejectionReasons;
            uu.Email = BoardReq.Email;
            uu.FullNameAr = BoardReq.FullNameAr;
            uu.MobileNumber = BoardReq.MobileNumber;
            uu.FullNameEn = BoardReq.FullNameEn;
            return uu;

        }

        
        

        public int AddBoardReq(BoardRequestsViewModel BoardReq)
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
                return 1;
            }
            catch (Exception ex) {
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



        public void CancelRequest(Guid id)
        {
            
            var BoardRequest = _context.tblBoardRequests.FirstOrDefault(r => r.Id == id);
            if (BoardRequest != null)
            {
                BoardRequest.RequestStatusId = Guid.Parse("11E42297-D061-42A0-B190-7D7B26936BAB"); // تعيين الحالة "تم الإلغاء"
                _context.SaveChanges();
            }
        }

        public void Accept(Guid id)
        {
            var BoardRequest = _context.tblBoardRequests.FirstOrDefault(r => r.Id == id);
            if (BoardRequest != null)
            {
                BoardRequest.RequestStatusId = Guid.Parse("59A1AE40-BF57-48AA-BF63-7672B679C152"); // تعيين الحالة "مقبول"
                _context.SaveChanges();
            }
        }

        public void Reject(Guid id , string rejectionReason)
        {
            var BoardRequest = _context.tblBoardRequests.FirstOrDefault(r => r.Id == id);
            if (BoardRequest != null)
            {
                BoardRequest.RequestStatusId = Guid.Parse("271A02AD-8510-406C-BEB4-832BF79159D4"); // تعيين الحالة "مرفوض"
                BoardRequest.RejectionReasons = rejectionReason;
                _context.SaveChanges();
            }
        }


    }
}
