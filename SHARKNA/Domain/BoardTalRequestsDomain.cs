
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            return _context.tblBoardTalRequests.Select(x => new BoardTalRequestsViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                BoardId = x.BoardId,
                BoardName = x.Board.NameAr,
                RejectionReasons = x.RejectionReasons,
                RequestStatusName = x.RequestStatus.RequestStatusAr,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                MobileNumber = x.MobileNumber,
                Skills = x.Skills,
                Experiences = x.Experiences,


            }).ToList();
        }

        public BoardTalRequestsViewModel GetTblBoardTalRequestsById(Guid id)
        {
            var Btal = _context.tblBoardTalRequests.FirstOrDefault(u => u.Id == id);
            BoardTalRequestsViewModel tt = new BoardTalRequestsViewModel();
            tt.Id = id;
            tt.UserName = Btal.UserName;
            tt.Email = Btal.Email;
            tt.BoardId = Btal.BoardId;
            tt.RejectionReasons = Btal.RejectionReasons;
            tt.RequestStatusId = Btal.RequestStatusId;
            tt.FullNameAr = Btal.FullNameAr;
            tt.MobileNumber = Btal.MobileNumber;
            tt.FullNameEn = Btal.FullNameEn;
            tt.Skills = Btal.Skills;
            tt.Experiences = Btal.Experiences;
            return tt;
        }

        public int AddUser(BoardTalRequestsViewModel user)
        {
            try
            {
                tblBoardTalRequests Vtal = new tblBoardTalRequests();
                Vtal.Id = user.Id;
                Vtal.UserName = user.UserName;
                Vtal.Email = user.Email;
                Vtal.BoardId = user.BoardId;
                Vtal.RequestStatusId = Guid.Parse("93d729fa-e7fa-4ea6-bb16-038454f8c5c2");
                Vtal.FullNameAr = user.FullNameAr;
                Vtal.FullNameEn = user.FullNameEn;
                Vtal.MobileNumber = user.MobileNumber;
                Vtal.Skills = user.Skills;
                Vtal.Experiences = user.Experiences;
                Vtal.RejectionReasons = user.RejectionReasons;


                _context.Add(Vtal);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public void CancelRequest(Guid id)
        {
            var Request = _context.tblBoardTalRequests.FirstOrDefault(r => r.Id == id);
            if (Request != null)
            {
                Request.RequestStatusId = Guid.Parse("e2d4f6a8-b2c3-4e5d-8a9f-0b1c2d3e4f5a"); // تعيين الحالة "تم الإلغاء"
                _context.SaveChanges();
            }
        }

        public void UpdateUser(BoardTalRequestsViewModel user)

        {
            tblBoardTalRequests Vtal = new tblBoardTalRequests();
            Vtal.Id = user.Id;
            Vtal.UserName = user.UserName;
            Vtal.Email = user.Email;
            Vtal.BoardId = user.BoardId;
            Vtal.FullNameAr = user.FullNameAr;
            Vtal.FullNameEn = user.FullNameEn;
            Vtal.MobileNumber = user.MobileNumber;
            Vtal.Skills = user.Skills;
            Vtal.Experiences = user.Experiences;
            Vtal.RejectionReasons = user.RejectionReasons;

            _context.tblBoardTalRequests.Update(Vtal);
            _context.SaveChanges();
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


    }
}

