using Microsoft.AspNetCore.Mvc;
using ProjektLAB.Models;
namespace ProjektLAB.Controllers
{
    public class DaneController : Controller
    {
        public IActionResult Index()

        {

            return View();
        }


        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Form(Dane dane)
        {
            return View("Wynik", dane);
        }
        public IActionResult Wynik(Dane dane)
        {

            return View(dane);
        }

    }
}
