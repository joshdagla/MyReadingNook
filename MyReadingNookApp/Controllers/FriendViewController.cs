using Microsoft.AspNetCore.Mvc;

namespace MyReadingNookApp.Controllers;

public class FriendViewController : Controller
{
    public IActionResult Invites()
    {
        return View();
    }
}
