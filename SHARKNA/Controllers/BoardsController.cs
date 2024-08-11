using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHARKNA.Domain;
using SHARKNA.Models;
using SHARKNA.ViewModels;
using System.Diagnostics;

namespace SHARKNA.Controllers
{
    public class BoardsController : Controller
    {
        private readonly BoardDomain _boardDomain;

        public BoardsController(BoardDomain boardDomain)
        {
            _boardDomain = boardDomain;
        }
        public IActionResult Index(string Successful = "",string Falied = "")
        {
            if (Successful != "")
                ViewData["Successful"] = Successful;
            else if (Falied != "")
                ViewData["Falied"] = Falied;



            var boards = _boardDomain.GetTblBoards();
            return View(boards);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BoardViewModel board)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_boardDomain.IsBoardNameDuplicate(board.NameEn))
                    {
                        ViewData["Falied"] = "اسم اللجنة مستخدم بالفعل";
                        return View(board);
                    }
                    board.Id = Guid.NewGuid();

                    int check = _boardDomain.AddBoard(board);
                    // return RedirectToAction(nameof(Index));
                    if (check == 1)
                        ViewData["Successful"] = "تم إضافة اللجنة بنجاح";
                    else
                        ViewData["Falied"] = "حدث خطأ بالإضافة";
                    return View(board);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء إضافة لجنة";
            }

            return View(board);
        }

        public IActionResult Update(Guid id)
        {
            var board = _boardDomain.GetTblBoardById(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BoardViewModel board)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (_boardDomain.IsBoardNameDuplicate(board.NameEn))
                    {
                        ViewData["Falied"] = "اسم اللجنة مستخدم بالفعل";
                        return View(board);
                    }
                    int check = _boardDomain.UpdateBoard(board);
                    if (check == 1)
                        ViewData["Successful"] = "تم التعديل بنجاح";
                    else
                        ViewData["Falied"] = "خطأ بالتعديل";
                    return View(board);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ في أثناء التعديل";
            }
            return View(board);
        }

        

        [HttpGet]
       // [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {
               

                int check = _boardDomain.DeleteBoard(id);
                if (check == 1)
                {
                     Successful = "تم حذف اللجنة بنجاح";
                }

                else
                {
                     Falied = "حدث خطأ";


                }
                //return View(id);

            }
            catch (Exception ex) {
                 Falied = "حدث خطأ";

            }
            //_boardDomain.DeleteBoard(id);
            return RedirectToAction(nameof(Index),new {Successful = Successful,  Falied = Falied});
        }

        //end delete

    }
}
