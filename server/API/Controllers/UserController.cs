using Microsoft.AspNetCore.Mvc;

namespace API.Configuration;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}