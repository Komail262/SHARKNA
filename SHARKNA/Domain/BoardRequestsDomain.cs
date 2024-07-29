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
                
                Email = x.Email,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                MobileNumber = x.MobileNumber
            }).ToList();
        }

        public BoardRequestsViewModel GetTblBoardRequestsById(Guid id)
        {
            var Req = _context.tblBoardRequests.FirstOrDefault(u => u.Id == id);
            BoardRequestsViewModel uu = new BoardRequestsViewModel();
            uu.Id = id;
            uu.UserName = Req.UserName;
            uu.BoardId = Req.BoardId;
            uu.Email = Req.Email;
            uu.FullNameAr = Req.FullNameAr;
            uu.MobileNumber = Req.MobileNumber;
            uu.FullNameEn = Req.FullNameEn;
            return uu;



        }

        public void AddBoardReq(BoardRequestsViewModel user)
        {
            tblBoardRequests Vuser = new tblBoardRequests();
            Vuser.Id = user.Id;
            Vuser.UserName = user.UserName;
            Vuser.BoardId = user.BoardId;
            Vuser.Email = user.Email;
            Vuser.FullNameAr = user.FullNameAr;
            Vuser.FullNameEn = user.FullNameEn;
            Vuser.MobileNumber = user.MobileNumber;

            _context.tblBoardRequests.Add(Vuser);
            _context.SaveChanges();
        }

        public void UpdateUser(BoardRequestsViewModel user)

        {
            tblBoardRequests Vuser = new tblBoardRequests();
            Vuser.Id = user.Id;
            Vuser.UserName = user.UserName;
            Vuser.BoardId = user.BoardId;
            Vuser.Email = user.Email;
            Vuser.FullNameAr = user.FullNameAr;
            Vuser.FullNameEn = user.FullNameEn;
            Vuser.MobileNumber = user.MobileNumber;

            _context.tblBoardRequests.Update(Vuser);
            _context.SaveChanges();
        }

        public bool IsEmailDuplicate(String email, Guid? userId = null)
        {
            if (userId == null)
            {
                return _context.tblBoardRequests.Any(u => u.Email == email);

            }
            else
            {
                return _context.tblBoardRequests.Any(u => u.Email == email && u.Id != userId);
            }
        }
    }
}
