using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;
namespace SHARKNA.Domain
{
    public class BoardMembersDomain
    {
        private readonly SHARKNAContext _context;
        private readonly BoardRequestsDomain _boardRequestsDomain;
        public BoardMembersDomain(SHARKNAContext context, BoardRequestsDomain boardRequestsDomain)
        {
            _context = context;
            _boardRequestsDomain = boardRequestsDomain;
        }


        public IEnumerable<BoardMembersViewModel> GetBoardMembersById()
        {
            return _context.tblBoardMembers
               
               .Select(x => new BoardMembersViewModel
               {
                   Id = x.Id,
                   UserName = x.UserName,
                   Email = x.Email,
                   MobileNumber = x.MobileNumber,
                   FullNameAr = x.FullNameAr,
                   FullNameEn = x.FullNameEn,
                   BoardId = x.BoardId,
                   BoardRoleId = x.BoardRoleId,
                   BoardRoleName = x.BoardRole.NameAr,
                   BoardName = x.Board.NameAr,

               }).ToList();
        }

        public async Task<IEnumerable<BoardMembersViewModel>> GetBoardMembersByBoardId(Guid boardId)
        {

            return await _context.tblBoardMembers.Include(b => b.Board).Include(br => br.BoardRole).Where(x => x.BoardId == boardId).Select(r => new BoardMembersViewModel
            {
            Id = r.Id,
            BoardId = r.BoardId,
            BoardName = r.Board.NameAr,
            UserName = r.UserName,
            Email = r.Email,
            MobileNumber = r.MobileNumber,
            FullNameAr = r.FullNameAr,
            FullNameEn = r.FullNameEn,
            BoardRoleId = r.BoardRoleId,
            BoardRoleName = r.BoardRole.NameAr,
            IsActive = r.IsActive,
            IsDeleted = r.IsDeleted,
            }).ToListAsync();
        }

        //public IEnumerable<BoardMembersViewModel> GetBoardMembersByBoardId(Guid boardId, Guid acceptedStatusId)
        //{
        //    return _context.tblBoardRequests


        //        .Where(x => x.BoardId == boardId && x.RequestStatusId == acceptedStatusId)
        //        .Select(x => new BoardMembersViewModel
        //        {
        //            Id = x.Id,
        //            UserName = x.UserName,
        //            Email = x.Email,
        //            MobileNumber = x.MobileNumber,
        //            FullNameAr = x.FullNameAr,
        //            FullNameEn = x.FullNameEn,
        //            BoardId = boardId, 
        //            BoardRoleId = Guid.Parse("7D67185D-81BD-4738-A6C5-2106E441EEA1"), 
        //           // BoardRoleName = x.,
        //           // IsActive = true,
        //           // IsDeleted = false ,

        //        })
        //        .ToList();
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




        //public IEnumerable<BoardMembersViewModel> GetBoardMembersByBoardId(Guid boardId, Guid acceptedStatusId)
        //{
        //    return _context.tblBoardRequests
        //        .Where(x => x.BoardId == boardId && x.RequestStatusId == acceptedStatusId)
        //        .Join(_context.tblBoardMembers,
        //              request => request.Id,  
        //              member => member.Id,              
        //              (request, member) => new BoardMembersViewModel
        //              {
        //                  Id = member.Id,
        //                  UserName = member.UserName,
        //                  Email = member.Email,
        //                  MobileNumber = member.MobileNumber,
        //                  FullNameAr = member.FullNameAr,
        //                  FullNameEn = member.FullNameEn,
        //                  BoardId = request.BoardId,
        //                  BoardName = request.Board.NameAr,
        //                  BoardRoleId = Guid.Parse("7D67185D-81BD-4738-A6C5-2106E441EEA1"),
        //                  BoardRoleName = member.BoardRole.NameAr,
        //                  IsActive = true ,
        //                  IsDeleted = false ,

        //              })
        //        .ToList();
        //}


        //public BoardMembersViewModel GetBoardMembersByBoardId(Guid boardId, Guid acceptedStatusId)
        //{
        //    return _context.tblBoardMembers
        //        .Include(x => x.tblBoardRequests)
        //        .Where(x => x.BoardId == boardId && x.RequestStatusId == acceptedStatusId)
           
               
        //        .Select(x => new BoardRequestsViewModel
        //        {
        //            Id = x.Id,
        //            FullNameAr = x.FullNameAr,
        //            UserName = x.UserName,
        //            Email = x.Email,
        //            MobileNumber = x.MobileNumber,
        //            BoardName = x.Board.NameAr,
        //            RequestStatusId = x.RequestStatusId,
        //            RequestStatusName = x.RequestStatus.RequestStatusAr,
        //            RejectionReasons = x.RejectionReasons
        //        })
        //        .FirstOrDefault();
        //}
    }

}

