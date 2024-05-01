using Microsoft.AspNetCore.Mvc;

namespace API.Configuration;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}