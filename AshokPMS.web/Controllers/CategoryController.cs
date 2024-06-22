using System.Runtime;
using AshokPMS.web.Data;
using AshokPMS.web.Models;
using AshokPMS.web.Services.Irepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AshokPMS.web.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : Controller
    {
        private readonly ICrudServices<Category> _category;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICrudServices<Category> category,
          UserManager<ApplicationUser> userManager)
        {
            _category = category;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoryList = await _category.GetAllAsync();
            return View(categoryList);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            Category category = new Category();
            category.IsActive = true;
            if (id > 0)
            {
                category = await _category.GetAsync(id);
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    if (category.Id == 0)
                    {
                        category.CreatedDate = DateTime.Now;
                        category.CreatedBy = userId;
                        await _category.InsertAsync(category);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var Orgcategory = await _category.GetAsync(category.Id);
                        Orgcategory.CategoryName = category.CategoryName;
                        Orgcategory.CategoryDescription = category.CategoryDescription;
                        Orgcategory.IsActive = category.IsActive;
                        Orgcategory.ModifiedDate = DateTime.Now;
                        Orgcategory.ModifiedBy = userId;
                        await _category.UpdateAsync(Orgcategory);
                        TempData["success"] = "Data Updated Sucessfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "Something went wrong, please try again later";
                    return RedirectToAction(nameof(AddEdit));
                }
            }

            TempData["error"] = "Please input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }

        public async Task<IActionResult> delete(int Id)
        {
            var category = await _category.GetAsync(Id);
            _category.Delete(category);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

    }
}
