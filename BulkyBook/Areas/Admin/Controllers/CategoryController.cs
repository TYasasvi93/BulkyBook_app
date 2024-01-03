using Booky.DataAccess.Data;
using Booky.DataAccess.Repository.IRepository;
using Booky.Model;
using Booky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The displayOrder cannot exactly match the name");
            }
            if (obj.Name?.ToLower() == "test")
            {
                ModelState.AddModelError("", "The 'test' is not a valid name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"]="Category Created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            //           Category? CategoryFromDb1 = _db.CategoryDb.FirstOrDefault(c => c.Id == id);
            //           Category? CategoryFromDb2 = _db.CategoryDb.Where(c => c.Id == id).FirstOrDefault();

            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The displayOrder cannot exactly match the name");
            }
            if (obj.Name?.ToLower() == "test")
            {
                ModelState.AddModelError("", "The 'test' is not a valid name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Editted Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            //           Category? CategoryFromDb1 = _db.CategoryDb.FirstOrDefault(c => c.Id == id);
            //           Category? CategoryFromDb2 = _db.CategoryDb.Where(c => c.Id == id).FirstOrDefault();

            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"]="Category Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
