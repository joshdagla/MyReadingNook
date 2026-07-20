using Microsoft.AspNetCore.Mvc;

namespace MyReadingNookApp.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}