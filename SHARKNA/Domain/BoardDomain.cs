using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class BoardDomain
    {
        private readonly SHARKNAContext _context;
        private readonly UserDomain _userDomain;

        public BoardDomain(SHARKNAContext context, UserDomain userDomain)
        {
            _context = context;
            _userDomain = userDomain;
        }

        public async Task<string> GetFullNameArByUsernameAsync(string username)
        {
            var user = await _context.tblUsers.FirstOrDefaultAsync(u => u.UserName == username);
            return user?.FullNameAr;
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
                Gender = x.Gender,
                IsDeleted = x.IsDeleted,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<BoardViewModel> GetTblBoardByIdAsync(Guid id)
        {
            var Fboard = await _context.tblBoards.FirstOrDefaultAsync(b => b.Id == id);
            if (Fboard == null) return null;
            BoardViewModel bb = new BoardViewModel();
            bb.Id = Fboard.Id;
            bb.NameAr = Fboard.NameAr;
            bb.NameEn = Fboard.NameEn;
            bb.DescriptionAr = Fboard.DescriptionAr;
            bb.DescriptionEn = Fboard.DescriptionEn;
            bb.Gender = Fboard.Gender;
            bb.IsDeleted = Fboard.IsDeleted;
            bb.IsActive = Fboard.IsActive;
            return bb;
            //};
            //BoardViewModel IBoard = new BoardViewModel();
        }

        public async Task<int> AddBoardAsync(BoardViewModel board, string username)
        {
            try
            {
                tblBoards Aboard = new tblBoards();

                Aboard.Id = board.Id;
                Aboard.NameAr = board.NameAr;
                Aboard.NameEn = board.NameEn;
                Aboard.DescriptionAr = board.DescriptionAr;
                Aboard.DescriptionEn = board.DescriptionEn;
                Aboard.Gender = board.Gender;
                Aboard.IsDeleted = false;
                Aboard.IsActive = true;


                await _context.tblBoards.AddAsync(Aboard);
                await _context.SaveChangesAsync(); // Save the board first
               
                
                tblBoardLogs blogs = new tblBoardLogs();
                blogs.Id = Guid.NewGuid();
                blogs.BrdId = Aboard.Id;
                blogs.OpType = "إضافة";
                blogs.OpDateTime = DateTime.Now;
                blogs.CreatedBy = username;
                blogs.CreatedTo = Aboard.NameAr;
                blogs.AdditionalInfo = $"تم إضافة لجنة {Aboard.NameAr} بواسطة هذا المستخدم {username}";
                _context.tblBoardLogs.Add(blogs);

                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }
        //changeing here for Update

        public async Task<int> UpdateBoardAsync(BoardViewModel board, string username)
        {
            try
            {
                //tblBoards Aboard = new tblBoards();
                //s new
                var Aboard = await _context.tblBoards.FirstOrDefaultAsync(b => b.Id == board.Id);
                if (Aboard == null)
                    return 0;//E new

                Aboard.Id = board.Id;
                Aboard.NameAr = board.NameAr;
                Aboard.NameEn = board.NameEn;
                Aboard.DescriptionAr = board.DescriptionAr;
                Aboard.DescriptionEn = board.DescriptionEn;
                Aboard.Gender = board.Gender;
                Aboard.IsDeleted = false;
                Aboard.IsActive = true;

                _context.tblBoards.Update(Aboard);
                await _context.SaveChangesAsync();

               
                tblBoardLogs blogs = new tblBoardLogs();
                blogs.Id = Guid.NewGuid();
                blogs.BrdId = Aboard.Id;
                blogs.OpType = "تعديل";
                blogs.OpDateTime = DateTime.Now;
                blogs.CreatedBy = username;
                blogs.CreatedTo = Aboard.NameAr;
                blogs.AdditionalInfo = $"تم تعديل لجنة {Aboard.NameAr} بواسطة هذا المستخدم {username}";
                _context.tblBoardLogs.Add(blogs);

                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }



        //End Update


        //delete



        public async Task<int> DeleteBoardAsync(Guid id, string username)
        {
            try
            {
                var board = await _context.tblBoards.FirstOrDefaultAsync(b => b.Id == id);
                if (board != null)
                {
                    board.IsDeleted = true;
                    board.IsActive = false;
                    _context.Update(board);
                    await _context.SaveChangesAsync();


                    tblBoardLogs blogs = new tblBoardLogs();
                    blogs.Id = Guid.NewGuid();
                    blogs.BrdId = board.Id;
                    blogs.OpType = "حذف";
                    blogs.OpDateTime = DateTime.Now;
                    blogs.CreatedBy = username;
                    blogs.CreatedTo = board.NameAr;
                    blogs.AdditionalInfo = $"تم حذف لجنة {board.NameAr} بواسطة هذا المستخدم {username}";
                    _context.tblBoardLogs.Add(blogs);

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


        //if nameAr duplicated

        //public async Task<bool> IsBoardNameDuplicateAsync(string name, Guid? Boardn = null)
        //{
        //    if (Boardn == null)
        //    {
        //        return await _context.tblBoards.AnyAsync(u => u.NameEn == name);

        //    }
        //    else
        //    {
        //        return await _context.tblBoards.AnyAsync(u => u.NameEn == name && u.Id != Boardn);
        //    }
        //}


    }
}