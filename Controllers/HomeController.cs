using System.Web.Mvc;
using AzWinApp_Web.Models;
using AzWinApp_Web.Security;

namespace AzWinApp_Web.Controllers
{
    [Authorize] // all actions require authentication
    public class HomeController : Controller
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();

        // LIST – visible to all three roles
        [Authorize(Roles = RoleNames.ReadOnly + "," + RoleNames.ReadWrite + "," + RoleNames.Admin)]
        public ActionResult Index()
        {
            var employees = _repo.GetAll();
            return View(employees);
        }

        // CREATE – only ReadWrite + Admin
        [Authorize(Roles = RoleNames.ReadWrite + "," + RoleNames.Admin)]
        public ActionResult Create()
        {
            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ReadWrite + "," + RoleNames.Admin)]
        public ActionResult Create(Employee model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return View(model);
            }

            _repo.Create(model);
            return RedirectToAction("Index");
        }

        // EDIT – only ReadWrite + Admin
        [Authorize(Roles = RoleNames.ReadWrite + "," + RoleNames.Admin)]
        public ActionResult Edit(int id)
        {
            var emp = _repo.GetById(id);
            if (emp == null) return HttpNotFound();
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ReadWrite + "," + RoleNames.Admin)]
        public ActionResult Edit(Employee model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return View(model);
            }

            _repo.Update(model);
            return RedirectToAction("Index");
        }

        // DELETE – Admin only
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Delete(int id)
        {
            var emp = _repo.GetById(id);
            if (emp == null) return HttpNotFound();
            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
