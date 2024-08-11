using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;
namespace SHARKNA.Domain
{
    public class BoardMembersDomain
    {
        private readonly SHARKNAContext _context;
        public BoardMembersDomain(SHARKNAContext context)
        {
            _context = context;
        }


        //public IEnumerable<BoardMembersViewModel> GetTblBoardMembers()
        //{
        //    return _context.tblBoardMembers.Select(x => new BoardMembersViewModel
        //    {
        //        Id = x.Id,
        //        FullNameAr = x.FullNameAr,
        //        FullNameEn = x.FullNameEn,
        //        Email = x.Email,
        //        BoardId = x.BoardId,
        //        BoardName = x.Board.NameAr,
        //        BoardRoleId = Guid.Parse("7D67185D-81BD-4738-A6C5-2106E441EEA1"),
        //        BoardRoleName = x.BoardRole.NameAr,
        //        MobileNumber = x.MobileNumber,
        //        IsDeleted = false,
        //        IsActive = true,
        //    }).ToList();
        //}

        public IEnumerable<BoardViewModel> GetTblBoards()
        {
            return _context.tblBoards.Where(i => i.IsDeleted == false).Select(x => new BoardViewModel
            {
                Id = x.Id,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                IsDeleted = x.IsDeleted,
                IsActive = x.IsActive
            }).ToList();

        }
       



        public IEnumerable<BoardMembersViewModel> GetBoardMembersByBoardId(Guid boardId, Guid acceptedStatusId)
        {
             return _context.tblBoardRequests
                .Where(x => x.BoardId == boardId && x.RequestStatusId == acceptedStatusId) 
                
                .Select(x => new BoardMembersViewModel 
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    MobileNumber = x.MobileNumber,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    BoardId = x.BoardId,
                    BoardName = x.Board.NameAr, 
                 
                }).ToList();
        }
    }

}
