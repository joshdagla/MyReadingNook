using Microsoft.AspNetCore.Mvc;

namespace MyReadingNookApp.Controllers;

public class BookDataController : Controller
{
    public IActionResult Details()
    {
        return View();
    }
}