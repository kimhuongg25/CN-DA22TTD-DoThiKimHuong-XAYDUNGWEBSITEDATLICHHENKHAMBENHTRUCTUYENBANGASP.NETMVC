using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;

namespace WebAppYte.Controllers
{
    public class TrungtamyteController : Controller
    {
        modelWeb db = new modelWeb();

        // GET: Trungtamyte
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen");
            return View(new List<TrungTamGanNhat>()); // Trả về list rỗng để không Null
        }

        // POST: Trungtamyte
        [HttpPost]
        public ActionResult Index(int IDHuyen)
        {
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen", IDHuyen);

            var tt = db.TrungTamGanNhats
                .Where(h => h.IDHuyen == IDHuyen)
                .Include(h => h.Huyen)
                .ToList();

            return View(tt);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var tt = db.TrungTamGanNhats.Include(t => t.Huyen).FirstOrDefault(t => t.IDTrungTam == id);
            if (tt == null) return HttpNotFound();

            return View(tt);
        }

    }


}