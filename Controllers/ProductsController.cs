using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;
using DmlCafePos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DmlCafePos.Controllers
{
    [Authorize(Roles = "admin,demo,tam_surum")]
    public class ProductsController : Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        public ProductsController(UserManager<AppUser> userManager, IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _userManager = userManager;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }
        public IActionResult ProductList()
        {
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var categoriesModel = new CategoryViewModel
                    {
                        Categories = _categoryRepo.Entites.Where(c => c.MandantID == user.MandantID)
                                                    .Include(c => c.Products)
                                                    .ToList(),
                        CategoryName = ""
                    };
                    return View(categoriesModel);
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category entity)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if (entity != null)
                    {
                        entity.MandantID = user.MandantID;
                        _categoryRepo.Create(entity);
                        return RedirectToAction("Categories");
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult> CategoryDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var category = _categoryRepo.Entites.FirstOrDefault(c => c.CategoryId == id && c.MandantID == user.MandantID);
                    if (category != null)
                    {
                        ViewBag.Categories = new SelectList(_categoryRepo.Entites.Where(c => c.MandantID == user.MandantID)
                                                        .Include(c => c.Products)
                                                        .ToList(),"CategoryId","CategoryName");
                        var categoriesModel = new CategoryViewModel
                        {
                            Category = category
                            
                        };
                        return View(categoriesModel);
                    }
                }
                return NotFound();
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var category = _categoryRepo.Entites.FirstOrDefault(c => c.CategoryId == id);
                    if (category != null && category.MandantID == user.MandantID)
                    {
                        _categoryRepo.Delete(id);
                        return RedirectToAction("Categories");
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductToDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var product = _productRepo.Entites.FirstOrDefault(p => p.ProductId == id && p.MandantID == user.MandantID);
                    if(product != null)
                    {
                        _productRepo.Delete(id);
                        return RedirectToAction("Categories");
                    }
                    return NotFound();
                }return NotFound();
            }return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductByCategory(Product entity,int categoryId,IFormFile imageFile)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if(entity.MandantID == null)
                    {
                        entity.MandantID = user.MandantID;
                    }
                    var category = _categoryRepo.Entites.FirstOrDefault(c => c.CategoryId == categoryId);
                    if(category == null)
                    {
                        return NotFound();
                    }
                    if(imageFile == null)
                    {
                         entity.Category = category;
                        _productRepo.Create(entity);
                        return RedirectToAction("Categories");
                    }
                    var extension = Path.GetExtension(imageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/image",randomFileName);
                    using(var stream = new FileStream(path,FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    entity.ImageUrl = randomFileName;
                    entity.Category = category;
                    _productRepo.Create(entity);
                    return RedirectToAction("Categories");
                    
                }
                return NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult>  AllProducts()
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    ViewBag.Categories = new SelectList(_categoryRepo.Entites.Where(c => c.MandantID == user.MandantID).ToList(),"CategoryId","CategoryName");
                                                      
                    var productModel = new ProductViewModel {
                        Products = _productRepo.Entites.Where(p => p.MandantID == user.MandantID)
                                                            .Include(p => p.Category)
                                                            .ToList()
                    };
                    
                    return View(productModel);
                }
                return NotFound();
            }return NotFound();
            
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(Product entity,int categoryId,IFormFile imageFile)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    if(entity.MandantID == null)
                    {
                        entity.MandantID = user.MandantID;
                    }
                    var category = _categoryRepo.Entites.FirstOrDefault(c => c.CategoryId == categoryId);
                    if(category == null)
                    {
                        return NotFound();
                    }
                    if(imageFile == null)
                    {
                        entity.Category = category;
                        _productRepo.Create(entity);
                        return RedirectToAction("AllProducts");
                    }
                    var extension = Path.GetExtension(imageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/image",randomFileName);
                    using(var stream = new FileStream(path,FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    entity.ImageUrl = randomFileName;
                    entity.Category = category;
                    _productRepo.Create(entity);
                    return RedirectToAction("AllProducts");
                }return NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult>  ProductDetails(int productId){
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var product = _productRepo.Entites.FirstOrDefault(p => p.ProductId == productId && p.MandantID == user.MandantID);
                    if(product != null)
                    {   
                        return View(product);
                    }return NotFound();
                }return NotFound();
            }return NotFound();
            
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(Product product,IFormFile? imageFile)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    if(imageFile != null)
                    {
                        var extension = Path.GetExtension(imageFile.FileName);
                        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/image",randomFileName);
                        using(var stream = new FileStream(path,FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        product.ImageUrl = randomFileName;
                        _productRepo.Update(product);
                        return RedirectToAction("AllProducts");
                    }else{
                        _productRepo.Update(product);
                        return RedirectToAction("AllProducts");
                    }
                    
                }return NotFound();
            }return NotFound();
            
        }
    }
}
