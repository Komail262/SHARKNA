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

        public void AddBoard(BoardViewModel board)
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
        }

        //changeing here for Update

        public void UpdateBoard(BoardViewModel board)
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
        }


        //public void UpdateBoard(BoardViewModel board) // will update board if not null 
        //{
        //    var Aboard = _context.tblBoards.FirstOrDefault(b => b.Id == board.Id);
        //    if (Aboard != null)
        //    {
        //        Aboard.NameAr = board.NameAr;
        //        Aboard.NameEn = board.NameEn;
        //        Aboard.DescriptionAr = board.DescriptionAr;
        //        Aboard.DescriptionEn = board.DescriptionEn;
        //        Aboard.IsActive = board.IsActive;
        //        Aboard.IsDeleted = board.IsDeleted;

        //        _context.tblBoards.Update(Aboard);
        //        _context.SaveChanges();
        //    }
        //}

        //End Update


        //delete

        //public void DeleteBoard(Guid id) // here will delete in view but will save in databse and change status of isdelete and isactive
        //{
        //    var board = _context.tblBoards.FirstOrDefault(b => b.Id == id);
        //    if (board != null)
        //    {
        //        board.IsDeleted = true;
        //        board.IsActive = false;
        //        _context.Update(board);
        //        _context.SaveChanges();
        //    }
        //}

        public void DeleteBoard(Guid id)
        {
            var board = _context.tblBoards.FirstOrDefault(b => b.Id == id);
            if (board != null)
            {
                board.IsDeleted = true;
                _context.Update(board);
                _context.SaveChanges();
            }
        }


    }
}
