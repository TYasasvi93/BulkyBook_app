using Booky.DataAccess.Repository.IRepository;
using Booky.Model;
using Booky.Model.ViewModels;
using Booky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().ToList().
            //    Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    }) ;
            //ViewBag.CategoryList = CategoryList;
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                Categorylist = _unitOfWork.Category.GetAll().ToList().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().ToList().
            //    Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            ////ViewBag.CategoryList = CategoryList;
            ////
            ////ViewData["CategoryList"] = CategoryList;

            ////ProductVM productVM = new()
            ////{
            ////    Categorylist = CategoryList,
            ////    Product = new Product()
            ////};
            ///
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (productVM.Product.Title?.ToLower() == "test")
            {
                ModelState.AddModelError("", "The 'test' is not a valid name");
            }
            if (ModelState.IsValid)
            {
                string wwwRootPatc = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPatc, @"images\product\");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePAth = Path.Combine(wwwRootPatc, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePAth))
                        {
                            System.IO.File.Delete(oldImagePAth);
                        }

                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product Created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product Updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.Categorylist = _unitOfWork.Category.GetAll().ToList().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);


            }

        }
        // Implemented in API
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    //           Product? productFromDb1 = _db.ProductDb.FirstOrDefault(c => c.Id == id);
        //    //           Product? productFromDb2 = _db.ProductDb.Where(c => c.Id == id).FirstOrDefault();

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    //TempData["success"]="Product deleted successfully";
        //    return RedirectToAction("Index");

        //}

        #region t API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data=objProductList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductToBeDeleted = _unitOfWork.Product.Get(u=>u.Id==id);
            if(ProductToBeDeleted == null)
            {
                return Json(new {succes=false,message="Error while Deleting"});
            }
            var oldImagePath=Path.Combine(_webHostEnvironment.WebRootPath,ProductToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(ProductToBeDeleted);
            _unitOfWork.Save();
            TempData["success"]="Product deleted successfully";
            return Json(new {success=true, message="Delete Successful"});
        }
        #endregion
    }
}
