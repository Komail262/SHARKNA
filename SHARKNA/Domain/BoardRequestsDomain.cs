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
                RejectionReasons = x.RejectionReasons,
    
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
            uu.RequestStatusId = BoardReq.RequestStatusId;
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

       
    }
}
