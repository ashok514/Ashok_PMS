using AshokPMS.web.Models;
using AshokPMS.web.Services.Irepository;
using Microsoft.AspNetCore.Identity;
using System.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace AshokPMS.web.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ProductController : Controller
    {
        private readonly ICrudServices<Product> _product;
        private readonly ICrudServices<Category> _category;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ICrudServices<Product> product,
            ICrudServices<Category> categoryInfo,
            UserManager<ApplicationUser> userManager)
        {
            
            _product = product;
            _category = categoryInfo;
            _userManager = userManager;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos = await _category.GetAllAsync(p => p.IsActive == true);

            var product = await _product.GetAllAsync();


            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int Id)
        {
            Product product = new Product();
            ViewBag.CategoryInfos = await _category.GetAllAsync(p => p.IsActive == true);
            product.IsActive = true;
            if (Id > 0)
            {
                product = await _product.GetAsync(Id);

            }

            return View(product);

        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product)
        {
            ViewBag.CategoryInfos = await _category.GetAllAsync(p => p.IsActive == true);

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);


                    if (product.ImageFile != null)
                    {
                        string fileDirectory = $"wwwroot/ProductImage";

                        if (!Directory.Exists(fileDirectory))
                        {
                            Directory.CreateDirectory(fileDirectory);
                        }
                        string uniqueFileName = Guid.NewGuid() + "_" + product.ImageFile.FileName;
                        string filePath = Path.Combine(Path.GetFullPath($"wwwroot/ProductImage"), uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                            product.ImageUrl = $"ProductImage/" + uniqueFileName;

                        }

                    }

                    if (product.Id == 0)
                    {
                        product.CreatedDate = DateTime.Now;
                        product.CreatedBy = userId;
                        await _product.InsertAsync(product);

                        TempData["success"] = "Data Added Successfully!";
                    }
                    else
                    {
                        var OrgProductInfo = await _product.GetAsync(product.Id);
                        OrgProductInfo.CategoryId = product.CategoryId;
                        OrgProductInfo.ProductName = product.ProductName;
                        OrgProductInfo.ProductDescription = product.ProductDescription;
                        OrgProductInfo.Manufacturer = product.Manufacturer;
                        OrgProductInfo.ProductionDate = product.ProductionDate;
                        OrgProductInfo.ExpiryDate = product.ExpiryDate;
                        OrgProductInfo.BatchNo = product.BatchNo;
                        OrgProductInfo.Price = product.Price;

                        if (product.ImageFile != null)
                        {
                            OrgProductInfo.ImageUrl = product.ImageUrl;
                        }

                        await _product.UpdateAsync(OrgProductInfo);
                        TempData["success"] = "Data Updated Successfully";
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
            var product = await _product.GetAsync(Id);
            _product.Delete(product);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}

