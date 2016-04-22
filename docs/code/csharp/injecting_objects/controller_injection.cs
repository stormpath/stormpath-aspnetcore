public class InjectedServicesController : Controller
{
    public IApplication StormpathApplication { get; private set; }

    public InjectedServicesController(IApplication stormpathApplication)
    {
        this.StormpathApplication = stormpathApplication;
    }

    public IActionResult Index()
    {
        return View();
    }
}
