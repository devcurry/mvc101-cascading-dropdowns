using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CascadingDropDown.Models
{
    public class AddressController : Controller
    {
        private CascadingDropdownSampleEntities db = new CascadingDropdownSampleEntities();

        //
        // GET: /Address/

        public ActionResult Index()
        {
            var addresses = db.addresses.Include(a => a.country).Include(a => a.state);
            return View(addresses.ToList());
        }

        //
        // GET: /Address/Details/5

        public ActionResult Details(int id = 0)
        {
            address address = db.addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        //
        // GET: /Address/Create

        public ActionResult Create()
        {
            ViewBag.country_id = new SelectList(db.countries, "country_id", "country_name");
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_name");
            return View();
        }

        public JsonResult SelectStates(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable<state> states = db.states.Where(stat => stat.country_id == id);
            return Json(states);
        }

        //
        // POST: /Address/Create

        [HttpPost]
        public ActionResult Create(address address)
        {
            if (ModelState.IsValid)
            {
                db.addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.country_id = new SelectList(db.countries, "country_id", "country_name", address.country_id);
            //ViewBag.state_id = new SelectList(db.states, "state_id", "state_name", address.state_id);
            return View(address);
        }

        //
        // GET: /Address/Edit/5

        public ActionResult Edit(int id = 0)
        {
            address address = db.addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.country_id = new SelectList(db.countries, "country_id", "country_name", address.country_id);
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_name", address.state_id);
            return View(address);
        }

        //
        // POST: /Address/Edit/5

        [HttpPost]
        public ActionResult Edit(address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.country_id = new SelectList(db.countries, "country_id", "country_name", address.country_id);
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_name", address.state_id);
            return View(address);
        }

        //
        // GET: /Address/Delete/5

        public ActionResult Delete(int id = 0)
        {
            address address = db.addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        //
        // POST: /Address/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            address address = db.addresses.Find(id);
            db.addresses.Remove(address);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}