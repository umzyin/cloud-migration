using AzWinApp_Web.Models;
using AzWinApp_Web.Security;
using System;
using System.Web.Mvc;

namespace AzWinApp_Web.Controllers
{
    [Authorize] // everyone must be authenticated
    public class TestItemsController : Controller
    {
        private readonly TestItemRepository _repo = new TestItemRepository();
        // READ – allowed to ALL 3 roles
        [Authorize(Roles = RoleNames.ReadOnly + "," + RoleNames.ReadWrite + "," + RoleNames.Admin)]
        public ActionResult Index()
        {
            var items = _repo.GetAll();
            var count = items == null ? 0 : System.Linq.Enumerable.Count(items);

            return Content(
                $"Index: {count} TestItem row(s) found in DB. Visible to ReadOnly, ReadWrite, Admin",
                "text/plain");
        }

        // CREATE – only ReadWrite + Admin
        [Authorize(Roles = RoleNames.ReadWrite + "," + RoleNames.Admin)]
        
        public ActionResult Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Content(
                    "Use /TestItems/Create?name=SomeName&description=OptionalText  (ReadWrite/Admin only)",
                    "text/plain");
            }

            var item = new TestItem
            {
                Name = name,
                Description = description,
                CreatedBy = User.Identity.Name,
                CreatedOn = DateTime.UtcNow
            };

            _repo.Insert(item);

            return Content(
                $"Created TestItem with Name='{item.Name}'. Now check /TestItems/Index to see the count increase.",
                "text/plain");
        }


        // EDIT – only ReadWrite + Admin
        [Authorize(Roles = RoleNames.ReadWrite + "," + RoleNames.Admin)]

        public ActionResult Edit(int id, string name, string description)
        {
            if (id <= 0)
            {
                return Content(
                    "Use /TestItems/Edit/1?name=NewName&description=NewDescription  (ReadWrite/Admin only)",
                    "text/plain");
            }

            var existing = _repo.GetById(id);
            if (existing == null)
            {
                return Content($"No TestItem found with Id={id}", "text/plain");
            }

            // If name/description not supplied, keep old values
            if (!string.IsNullOrWhiteSpace(name))
                existing.Name = name;

            if (description != null)
                existing.Description = description;

            _repo.Update(existing);

            return Content(
                $"Updated TestItem Id={id}. Now check /TestItems/Index to see the changes.",
                "text/plain");
        }

        // DELETE – Admin only
        [Authorize(Roles = RoleNames.Admin)]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return Content(
                    "Use /TestItems/Delete/1  (Admin only)",
                    "text/plain");
            }

            var existing = _repo.GetById(id);
            if (existing == null)
            {
                return Content($"No TestItem found with Id={id}", "text/plain");
            }

            _repo.Delete(id);

            return Content(
                $"Deleted TestItem Id={id}. Now check /TestItems/Index to see the count decrease.",
                "text/plain");
        }

    }
}
