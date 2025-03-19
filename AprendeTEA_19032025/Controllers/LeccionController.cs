using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AprendeTEA_19032025.Controllers
{
    public class LeccionController : Controller
    {
        // GET: LeccionController
        public ActionResult Lecciones()
        {
            return View();
        }

        // GET: LeccionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeccionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeccionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeccionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeccionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeccionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeccionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
