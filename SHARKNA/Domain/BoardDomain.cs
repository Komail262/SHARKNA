﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> AddBoardAsync(BoardViewModel board)
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

                //if (_context.SaveChanges() > 0)
                //{
                //    tblBoardLogs bLogs = new tblBoardLogs();
                //    bLogs.Id = Aboard.Id;
                //    bLogs.OpType = "Add";
                //    bLogs.OpDateTime = DateTime.Now;
                //    bLogs.CreatedBy = "000";
                //    bLogs.CreatedTo = Aboard.NameAr;
                //    bLogs.AdditionalInfo = $"تم إضافة لجنة {Aboard.NameAr} بواسطة هذا المستخدم";
                //    _context.Boardlogs.Add(bLogs);
                //}


                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }
        //changeing here for Update

        public async Task<int> UpdateBoardAsync(BoardViewModel board)
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


                //if (_context.SaveChanges() > 0)
                //{
                //    tblBoardLogs bLogs = new tblBoardLogs();
                //    bLogs.Id = Aboard.Id;
                //    bLogs.OpType = "Update";
                //    bLogs.OpDateTime = DateTime.Now;
                //    bLogs.CreatedBy = "000";
                //    bLogs.CreatedTo = Aboard.NameAr;
                //    bLogs.AdditionalInfo = $"تم تعديل لجنة {Aboard.NameAr} بواسطة هذا المستخدم";
                //    _context.Boardlogs.Update(bLogs);
                //}


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



        public async Task<int> DeleteBoardAsync(Guid id)
        {
            try
            {
                var board = await _context.tblBoards.FirstOrDefaultAsync(b => b.Id == id);
                if (board != null)
                {
                    board.IsDeleted = true;
                    board.IsActive = false;
                    _context.Update(board);

                    //if (_context.SaveChanges() > 0)
                    //{
                    //    tblBoardLogs bLogs = new tblBoardLogs();
                    //    bLogs.Id = board.Id;
                    //    bLogs.OpType = "Delete";
                    //    bLogs.OpDateTime = DateTime.Now;
                    //    bLogs.CreatedBy = "000";
                    //    bLogs.CreatedTo = board.NameAr;
                    //    bLogs.AdditionalInfo = $"تم حذف لجنة {board.NameAr} بواسطة هذا المستخدم";
                    //    _context.Boardlogs.update(bLogs);
                    //}

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

        public async Task<bool> IsBoardNameDuplicateAsync(string name, Guid? Boardn = null)
        {
            if (Boardn == null)
            {
                return await _context.tblBoards.AnyAsync(u => u.NameEn == name);

            }
            else
            {
                return await _context.tblBoards.AnyAsync(u => u.NameEn == name && u.Id != Boardn);
            }
        }


    }
}