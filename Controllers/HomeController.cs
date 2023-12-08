using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektLAB.Areas.Identity.Data;
using ProjektLAB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjektLAB.Models.Dane;

namespace ProjektLAB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Add admin user
            await CreateAdminUser();

            var cars = _context.Cars.ToList();

            return View(cars);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var car = _context.Cars.Find(id);
            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            // Provide a form to add a new car
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(Cars car)
        {
            var categoryExists = _context.Categories.Any(c => c.CategoryId == car.CategoryId);
            ViewBag.Categories = _context.Categories.ToList();

            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Invalid Category selected.");
                return View(car);
            }

            var selectedCategory = _context.Categories.Find(car.CategoryId);
            if (selectedCategory != null)
            {
                car.CategoryName = selectedCategory.CategoryName;
            }
            // Add a new car to the database
            _context.Cars.Add(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            var car = _context.Cars.Find(id);
            return View(car);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Cars car)
        {
            var categoryExists = _context.Categories.Any(c => c.CategoryId == car.CategoryId);
            ViewBag.Categories = _context.Categories.ToList();

            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Invalid Category selected.");
                return View(car);
            }

            var selectedCategory = _context.Categories.Find(car.CategoryId);
            if (selectedCategory != null)
            {
                car.CategoryName = selectedCategory.CategoryName;
            }
            // Update an existing car in the database
            _context.Cars.Update(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            // Provide a confirmation page to delete a car
            var car = _context.Cars.Find(id);
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete an existing car from the database
            var car = _context.Cars.Find(id);
            _context.Cars.Remove(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private async Task CreateAdminUser()
        {
            string roleName = "Administrator";
            IdentityResult roleResult;

            // Check if the Administrator role already exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                // If not, create the role
                roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Check if the admin user already exists
            var user = await _userManager.FindByEmailAsync("admin@example.com");

            if (user == null)
            {
                // If not, create a new user
                user = new ApplicationUser()
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                };
                await _userManager.CreateAsync(user, "Admin123@");
            }

            // Add the admin user to the Administrator role if not already a member
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
