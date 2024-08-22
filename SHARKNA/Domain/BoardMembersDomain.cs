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




       
    }

}
