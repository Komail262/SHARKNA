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


    }
}
