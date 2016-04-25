using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReservaDeAulas.Models;

namespace ReservaDeAulas.Controllers
{
    public class DisponiblesController : Controller
    {
        private ReservaAulaEntities db = new ReservaAulaEntities();

        // GET: Disponibles
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var disponible = db.Disponible.Include(d => d.Aula);
                return View(disponible.ToList());
            }
            else
            {
                var disponiblee = db.Disponible.Where(x=>x.Estado==true).Include(d => d.Aula); ;
                //var disponible = db.Disponible.Include(d => d.Aula);
                return View(disponiblee.ToList());
            }
            
        }

        // GET: Disponibles/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponible disponible = db.Disponible.Find(id);
            if (disponible == null)
            {
                return HttpNotFound();
            }
            return View(disponible);
        }

        // GET: Disponibles/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdAula = new SelectList(db.Aula, "IdAula", "Descripcion");
            return View();
        }

        // POST: Disponibles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDisponible,IdAula,Fecha,Estado")] Disponible disponible)
        {
            if (ModelState.IsValid)
            {
                db.Disponible.Add(disponible);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAula = new SelectList(db.Aula, "IdAula", "Descripcion", disponible.IdAula);
            return View(disponible);
        }

        // GET: Disponibles/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponible disponible = db.Disponible.Find(id);
            if (disponible == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAula = new SelectList(db.Aula, "IdAula", "Descripcion", disponible.IdAula);
            return View(disponible);
        }

        // POST: Disponibles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdDisponible,IdAula,Fecha,Estado")] Disponible disponible)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disponible).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAula = new SelectList(db.Aula, "IdAula", "Descripcion", disponible.IdAula);
            return View(disponible);
        }

        // GET: Disponibles/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponible disponible = db.Disponible.Find(id);
            if (disponible == null)
            {
                return HttpNotFound();
            }
            return View(disponible);
        }

        // POST: Disponibles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Disponible disponible = db.Disponible.Find(id);
            db.Disponible.Remove(disponible);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
