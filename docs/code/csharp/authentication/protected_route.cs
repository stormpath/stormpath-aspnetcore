[Authorize]
public class ProfileController : Controller
{
    // GET: /profile
    public IActionResult Index()
    {
        // [Authorize] will require a logged-in user for this action
        return View();
    }
}
