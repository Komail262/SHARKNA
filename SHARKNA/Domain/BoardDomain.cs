using System.Collections.Generic;
using System.Linq;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class BoardDomain
    {
        private readonly SHARKNAContext _context;
        public BoardDomain(SHARKNAContext context)
        {
            _context = context;
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

        public BoardViewModel GetTblBoardById(Guid id)
        {
            var Fboard = _context.tblBoards.FirstOrDefault(b => b.Id == id);
            if (Fboard == null) return null;
            BoardViewModel bb = new BoardViewModel();
            bb.Id = Fboard.Id;
            bb.NameAr = Fboard.NameAr;
            bb.NameEn = Fboard.NameEn;
            bb.DescriptionAr = Fboard.DescriptionAr;
            bb.DescriptionEn = Fboard.DescriptionEn;
            bb.IsDeleted = Fboard.IsDeleted;
            bb.IsActive = Fboard.IsActive;
            return bb;
            //};
            //BoardViewModel IBoard = new BoardViewModel();
        }

        public int AddBoard(BoardViewModel board)
        {
            try
            {
                tblBoards Aboard = new tblBoards();

                Aboard.Id = board.Id;
                Aboard.NameAr = board.NameAr;
                Aboard.NameEn = board.NameEn;
                Aboard.DescriptionAr = board.DescriptionAr;
                Aboard.DescriptionEn = board.DescriptionEn;
                Aboard.IsDeleted = false;
                Aboard.IsActive = true;

                _context.tblBoards.Add(Aboard);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }
        //changeing here for Update

        public int UpdateBoard(BoardViewModel board)
        {
            try
            {
                tblBoards Aboard = new tblBoards();

                Aboard.Id = board.Id;
                Aboard.NameAr = board.NameAr;
                Aboard.NameEn = board.NameEn;
                Aboard.DescriptionAr = board.DescriptionAr;
                Aboard.DescriptionEn = board.DescriptionEn;
                Aboard.IsDeleted = false;
                Aboard.IsActive = true;

                _context.tblBoards.Update(Aboard);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }



        //End Update


        //delete



        public int DeleteBoard(Guid id)
        {
            try
            {
                var board = _context.tblBoards.FirstOrDefault(b => b.Id == id);
                if (board != null)
                {
                    board.IsDeleted = true;
                    board.IsActive = false;
                    _context.Update(board);
                    _context.SaveChanges();

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

        public bool IsBoardNameDuplicate(string name, Guid? Boardn = null)
        {
            if (Boardn == null)
            {
                return _context.tblBoards.Any(u => u.NameEn == name);

            }
            else
            {
                return _context.tblBoards.Any(u => u.NameEn == name && u.Id != Boardn);
            }
        }


    }
}