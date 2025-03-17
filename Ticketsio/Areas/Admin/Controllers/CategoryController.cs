using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
           var categories = categoryRepository.Get();
            return View(categories.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddSeconds(10),
                    Secure = true
                };
                Response.Cookies.Append("notification", "Created Category Successfuly", cookieOptions);
                return RedirectToAction("Index");
            }
           return View(category);
        }
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetOne(e => e.Id == id);
            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
          var category =  categoryRepository.GetOne(e => e.Id == id);
        return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Edit(category);
                categoryRepository.Commit();
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddSeconds(10),
                    Secure = true
                };
                Response.Cookies.Append("notification", "Edited Category Successfuly", cookieOptions);
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
