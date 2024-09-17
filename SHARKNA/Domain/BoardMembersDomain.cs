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

        public async Task<BoardMembersViewModel> GetBoardMemberByIdAsync(Guid id)
        {
            return await _context.tblBoardMembers
                .Where(x => x.Id == id)
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
                }).FirstOrDefaultAsync();
        }

        public async Task<tblBoardMembers> GettblBoardMemberByIdAsync(Guid id)
        {
            return await _context.tblBoardMembers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<BoardMembersViewModel>> GetBoardMembersByBoardId(Guid boardId)
        {

            return await _context.tblBoardMembers
                .Include(b => b.Board)
                .Include(br => br.BoardRole)
                .Where(x => x.BoardId == boardId && x.IsDeleted == false)
                .Select(r => new BoardMembersViewModel
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
        public async Task<int> UpdateBoardMembersAsync(tblBoardMembers MemR)
        {
            try
            {
                var MemberRole = await GettblBoardMemberByIdAsync(MemR.Id);
                MemberRole.BoardRoleId = MemR.BoardRoleId;
                _context.Update(MemberRole);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //public async Task<int> UpdateBoardMembersAsync(Guid id)
        //{
        //    try
        //    {
        //        var Mem = await _context.tblBoardMembers.FirstOrDefaultAsync(b => b.Id == id);
        //        if (Mem != null)
        //        {
        //            Mem.BoardRoleId = id;
        //            _context.Update(Mem);
        //            await _context.SaveChangesAsync();

        //            return 1;
        //        }
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }

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


            public async Task<int> DeleteBoardMembersAsync(Guid id)
            {
                try
                {
                    var board = await _context.tblBoardMembers.FirstOrDefaultAsync(b => b.Id == id);
                    if (board != null)
                    {
                        board.IsDeleted = true;
                        board.IsActive = false;
                        _context.Update(board);
                        await _context.SaveChangesAsync();

                        return 1;
                    }
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }




        }

    }

