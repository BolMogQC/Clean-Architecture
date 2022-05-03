using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}