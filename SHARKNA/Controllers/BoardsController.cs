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

        public async Task<IActionResult> Update(Guid id)
        {
            var board = await _boardDomain.GetTblBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardViewModel board)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _boardDomain.IsBoardNameDuplicateAsync(board.NameEn))
                    {
                        ViewData["Falied"] = "اسم اللجنة مستخدم بالفعل";
                        return View(board);
                    }
                    board.Id = Guid.NewGuid();

                    int check = await _boardDomain.AddBoardAsync(board);
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

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Update(BoardViewModel board)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Eboard = await _boardDomain.GetTblBoardByIdAsync(board.Id);//ياخذ بيانات اللجنة الحالموجودة حاليا من الداتابيس باستخدام ID قبل اي تعديل  

                    if (Eboard != null && Eboard.NameEn != board.NameEn && await _boardDomain.IsBoardNameDuplicateAsync(board.NameEn))//new
                        // Eboard != null يتأكد اللجنة موجودة ولا 
                        //Eboard.NameEn != board.NameEn يتأكد اذا كنت تبي تغير الاسم بالانجليزي وهل اذا غيرته بيكون مختلف اذا مختلف بيكمل 
                        // _boardDomain.IsBoardNameDuplicate(board.NameEn) يتأكد هل الاسم الجديد موجود بالداتابيس 


                    //if (_boardDomain.IsBoardNameDuplicate(board.NameEn))
                    {
                        ViewData["Falied"] = "اسم اللجنة مستخدم بالفعل";
                        return View(board);
                    }
                    int check = await _boardDomain.UpdateBoardAsync(board);
                    if (check == 1)
                        ViewData["Successful"] = "تم التعديل بنجاح";
                    else
                        ViewData["Falied"] = "خطأ بالتعديل";
                    return View(board);

                }
            }
            catch (Exception ex)
            {
                ViewData["Falied"] = "حدث خطأ أثناء التعديل";
            }
            return View(board);
        }

        

        [HttpGet]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            string Successful = "";
            string Falied = "";
            try
            {
               

                int check = await _boardDomain.DeleteBoardAsync(id);
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
