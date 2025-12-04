using System.Web.Mvc;
using AzWinApp.Models;

namespace AzWinApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userName = User?.Identity?.Name ?? "Unknown";

            var repo = new EmployeeRepository();
            var employees = repo.GetAll();

            var vm = new HomeViewModel
            {
                CurrentUser = userName,
                Employees = employees
            };

            return View(vm);
        }
    }

    public class HomeViewModel
    {
        public string CurrentUser { get; set; }
        public System.Collections.Generic.IEnumerable<Employee> Employees { get; set; }
    }
}
