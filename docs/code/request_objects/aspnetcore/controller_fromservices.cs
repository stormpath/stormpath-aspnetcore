public class ServicesController : Controller
{
    [FromServices]
    public IApplication StormpathApplication { get; set; }

    public IActionResult Index()
    {
        return View();
    }
}
