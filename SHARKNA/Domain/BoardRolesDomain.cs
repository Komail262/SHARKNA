﻿using System.Collections.Generic;
using System.Linq;
using SHARKNA.Models;
using SHARKNA.ViewModels;

namespace SHARKNA.Domain
{
    public class BoardRolesDomain
    {
        private readonly SHARKNAContext _context;
        public BoardRolesDomain(SHARKNAContext context)
        {
            _context = context;
        }
//m
        public IEnumerable<BoardRolesViewModel> GettbBoardRoles()
        {
            return _context.tblBoardRoles.Where(i => i.IsDeleted == false).Select(boardRoles => new BoardRolesViewModel
            {

                Id = boardRoles.Id,
                NameAr = boardRoles.NameAr,
                NameEn = boardRoles.NameEn,
                IsActive = boardRoles.IsActive,
                IsDeleted = boardRoles.IsDeleted,


            }).ToList();
        }

        public BoardRolesViewModel GetTblBoardRolesById(Guid id)
        {
            var boardRoles = _context.tblBoardRoles.FirstOrDefault(u => u.Id == id);
            BoardRolesViewModel uu = new BoardRolesViewModel();
            uu.Id = id;
            uu.NameAr = boardRoles.NameAr;
          uu.NameEn = boardRoles.NameEn; 
            uu.IsActive = boardRoles.IsActive;
            uu.IsDeleted = boardRoles.IsDeleted;
            return uu;

        }

        public void AddBoardRoles(BoardRolesViewModel boardRoles)
        {
            tblBoardRoles VboardRoles = new tblBoardRoles();
            VboardRoles.Id = boardRoles.Id;
            VboardRoles.NameAr = boardRoles.NameAr;
            VboardRoles.NameEn = boardRoles.NameEn;
            VboardRoles.IsActive = boardRoles.IsActive;
            VboardRoles.IsDeleted = boardRoles.IsDeleted;


            _context.tblBoardRoles.Add(VboardRoles);
            _context.SaveChanges();
        }

        public int UpdateBoardRoles(BoardRolesViewModel boardRoles)
        {
            try
            {
                var existingBoardRole = _context.tblBoardRoles.FirstOrDefault(e => e.Id == boardRoles.Id);
                if (existingBoardRole == null)
                {
                    return 0;
                }

                existingBoardRole.NameAr = boardRoles.NameAr;
                existingBoardRole.NameEn = boardRoles.NameEn;
                
                existingBoardRole.IsDeleted = false;
                existingBoardRole.IsActive = true;

                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public int DeleteBoaRoles(Guid id)
        {
            try
            {
                var boardRoles = _context.tblBoardRoles.FirstOrDefault(b => b.Id == id);
                if (boardRoles != null)
                {
                    boardRoles.IsDeleted = true;
                    boardRoles.IsActive = false;
                    _context.Update(boardRoles);
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

    }
}
