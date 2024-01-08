using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektLAB.Areas.Identity.Data;
using ProjektLAB.Models;
using ProjektLAB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
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
            await CreateAdminUser();

            var cars = _context.Cars.ToList();

            return View(cars);
        }

        // Wyświetlanie samochodów na stronie
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
            return View();
        }

        // Dodawanie samochodów przez admina
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
            
            _context.Cars.Add(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reservation(int id)
        {
            var car = _context.Cars.Find(id);

            if (car == null)
            {
                ModelState.AddModelError("CarId", "Invalid car selected.");
                return View(car);
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);

            var viewModel = new ReservationViewModel
            {
                Car = car,
                Client = new Clients(), 
                User = user,
            };

            return View("Reservation", viewModel);
        }

        // Logika rezerwacji dla użytkowników zalogowanych i nie zalogowanych
        [HttpPost]
        public IActionResult Reservation(ReservationViewModel viewModel)
        {

            int carId = viewModel.Car.CarId;
            var existingCar = _context.Cars.Find(carId);
            viewModel.Car = existingCar;
            var category = viewModel.Car.Category;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (User.Identity.IsAuthenticated) 
            {
               
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                ApplicationUser user = new ApplicationUser
                {
                    Id = userId,
                    FirstName = viewModel.User.FirstName,
                    LastName = viewModel.User.LastName,
                    Email = viewModel.User.Email,
                    Street = viewModel.User.Street,
                    Postcode = viewModel.User.Postcode,
                    City = viewModel.User.City,
                };


                Orders order = new Orders
                {
                    OrderDate = DateTime.Now,
                    UserId = viewModel.User.Id,
                    ClientId = null,
                    CarId = carId,
                    PickupDate = viewModel.Order.PickupDate,
                    ReturnDate = viewModel.Order.ReturnDate
                };

                var loggedInResultViewModel = new ReservationViewModel
                {
                    Car = existingCar,
                    User = user,
                    Order = order
                };

                // Add the order to the database
                _context.Orders.Add(order);
                _context.SaveChanges();

                return View("ReservationResult", loggedInResultViewModel);
            } else 
            {
                var client = new Clients
                {
                    ClientId = Guid.NewGuid().ToString(),
                    FirstName = viewModel.Client.FirstName,
                    LastName = viewModel.Client.LastName,
                    Street = viewModel.Client.Street,
                    PostCode = viewModel.Client.PostCode,
                    City = viewModel.Client.City,
                    PhoneNumber = viewModel.Client.PhoneNumber,
                    Email = viewModel.Client.Email,
                };

                _context.Clients.Add(client);
                _context.SaveChanges();

                if (existingCar == null)
                {
                    ModelState.AddModelError("CategoryId", "Invalid car selected.");
                    return View(existingCar);
                }

                Orders order = new Orders
                {
                    OrderDate = DateTime.Now,
                    ClientId = client.ClientId,
                    UserId = null,
                    CarId = carId,
                    PickupDate = viewModel.Order.PickupDate,
                    ReturnDate = viewModel.Order.ReturnDate
                };

                var notLoggedInResultViewModel = new ReservationViewModel
                {
                    Client = client,
                    Car = existingCar,
                    Order = order
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
             
                return View("ReservationResult", notLoggedInResultViewModel);
            }
        }

        // Wyświetlanie wyniku rezerwacji
        public IActionResult ReservationResult(ReservationViewModel viewModel)
        {
            return View(viewModel);
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            var car = _context.Cars.Find(id);
            return View(car);
        }
        
        // Edycja samochodów przez admina
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
            _context.Cars.Update(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var car = _context.Cars.Find(id);
            return View(car);
        }

        // Usuwanie samochodów przez admina
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _context.Cars.Find(id);
            _context.Cars.Remove(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Tworzenie administratora
        private async Task CreateAdminUser()
        {
            string roleName = "Administrator";
            IdentityResult roleResult;

            // Sprawdzanie czy rola administatora istnieje
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Sprawdzanie czy użytkownik "administator" istnieje
            var user = await _userManager.FindByEmailAsync("admin@admin.pl");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "admin@admin.pl",
                    Email = "admin@admin.pl",
                };
                await _userManager.CreateAsync(user, "Admin123@");
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
