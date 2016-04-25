using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReservaDeAulas.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ReservaDeAulas.Controllers
{
    public class ReservasController : Controller
    {
        private ReservaAulaEntities db = new ReservaAulaEntities();

        // GET: Reservas
        [Authorize]
        public ActionResult Index()
        {
            var reserva = db.Reserva.Include(r => r.Disponible);
            return View(reserva.ToList());
        }

        // GET: Reservas/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // GET: Reservas/Create
        //[AllowAnonymous]
        //public ActionResult Create()
        //{
        //    ViewBag.IdDisponible = new SelectList(db.Disponible, "IdDisponible", "IdDisponible");
        //    return View();
        //}
        // GET: Reservas/Create/1
        [AllowAnonymous]
        public ActionResult Create(int? id)
        {


            Disponible disponible = db.Disponible.Find(id);
            if (disponible == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdDisponible = new SelectList(db.Disponible.Where(x=>x.IdDisponible==id), "IdDisponible", "IdDisponible");
            ViewBag.FechaDisponible = disponible.Fecha;
            ViewBag.Codigo = disponible.Aula.Descripcion;
            ViewBag.Salon = disponible.Aula.NumAula;
            ViewBag.Bloque = disponible.Aula.Bloque;


            return View();

        }
        // POST: Reservas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "IdReserva,IdDisponible,Nombre,Email")] Reserva reserva)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Reserva.Add(reserva);
                    db.Disponible.Find(reserva.IdDisponible).Estado = false;
                    db.SaveChanges();
                    SendSimpleMessage(reserva);
                    if (User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Disponibles");
                    }

                }
                Disponible disponible = db.Disponible.Find(reserva.IdDisponible);
                if (disponible == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdDisponible = new SelectList(db.Disponible.Where(x => x.IdDisponible == reserva.IdDisponible), "IdDisponible", "IdDisponible");
                ViewBag.FechaDisponible = disponible.Fecha;
                ViewBag.Codigo = disponible.Aula.Descripcion;
                ViewBag.Salon = disponible.Aula.NumAula;
                ViewBag.Bloque = disponible.Aula.Bloque;
                return View(reserva);
            }
            catch (Exception e) {

                Disponible disponible = db.Disponible.Find(reserva.IdDisponible);
                if (disponible == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdDisponible = new SelectList(db.Disponible.Where(x => x.IdDisponible == reserva.IdDisponible), "IdDisponible", "IdDisponible");
                ViewBag.FechaDisponible = disponible.Fecha;
                ViewBag.Codigo = disponible.Aula.Descripcion;
                ViewBag.Salon = disponible.Aula.NumAula;
                ViewBag.Bloque = disponible.Aula.Bloque;
                return View(reserva);
            }
                
            

           
        }
        [Authorize]
        // GET: Reservas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDisponible = new SelectList(db.Disponible, "IdDisponible", "IdDisponible", reserva.IdDisponible);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdReserva,IdDisponible,Nombre,Email")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDisponible = new SelectList(db.Disponible, "IdDisponible", "IdDisponible", reserva.IdDisponible);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reserva.Find(id);
            db.Reserva.Remove(reserva);
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
        public static void SendSimpleMessage(Reserva reserva)
        {
            ReservaAulaEntities db = new ReservaAulaEntities();
            RestClient client = new RestClient();
            System.Uri uri = new System.Uri("https://api.mailgun.net/v3");
            client.BaseUrl = uri;
            client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-05394996027f04202e30a49f7d6ea57a");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "sandbox2a19a8beb07d48279eecf180fe4f0d62.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Equipo Reservas <postmaster@sandbox2a19a8beb07d48279eecf180fe4f0d62.mailgun.org>");
            request.AddParameter("to", reserva.Nombre + " " + "<" + reserva.Email + ">");
            request.AddParameter("subject", "Reserva de aula para: " + reserva.Nombre);
            request.AddParameter("text", "Has realizado una reserva de Aula\nDatos de la reserva:\n\tA nombre de: " + reserva.Nombre + "\n\tCódigo reserva: " + reserva.IdReserva + "\n\tFecha reserva: " + db.Disponible.FirstOrDefault(x => x.IdDisponible.Equals(reserva.IdDisponible)).Fecha + "\n\tSalon: " + db.Aula.FirstOrDefault(x=>x.IdAula==(db.Disponible.FirstOrDefault(y => y.IdDisponible.Equals(reserva.IdDisponible)).IdAula)).NumAula+ "\n\tBloque: " + db.Aula.FirstOrDefault(x => x.IdAula == (db.Disponible.FirstOrDefault(y => y.IdDisponible.Equals(reserva.IdDisponible)).IdAula)).Bloque + "\nRecuerde que el código de la reserva es indispensable para el ingreso.\nPara mayores informes llamar al 01-8000-117711");
            request.Method = Method.POST;
            client.Execute(request);
        }
    }
}
