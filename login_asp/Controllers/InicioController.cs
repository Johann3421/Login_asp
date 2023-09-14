using Microsoft.AspNetCore.Mvc;

namespace login_asp.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
