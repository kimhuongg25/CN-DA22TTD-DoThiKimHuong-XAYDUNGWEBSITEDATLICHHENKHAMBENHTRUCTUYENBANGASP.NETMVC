using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;

namespace WebAppYte.Areas.Admin.Controllers
{
    public class QuanTrisController : Controller
    {
        private modelWeb db = new modelWeb();

        // GET: Admin/QuanTris
        public ActionResult Index()
        {
            var quanTris = db.QuanTris.Include(q => q.Khoa);
            return View(quanTris.ToList());
        }

        // GET: Admin/QuanTris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // GET: Admin/QuanTris/Create
        public ActionResult Create()
        {
            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa");
            return View();
        }

        // POST: Admin/QuanTris/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDQuanTri,TaiKhoan,MatKhau,VaiTro,ThongTinBacSi,TrinhDo,IDKhoa,HoTen")] QuanTri quanTri,
                                   HttpPostedFileBase uploadAnhBia)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh
                if (uploadAnhBia != null && uploadAnhBia.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(uploadAnhBia.FileName);
                    string path = Server.MapPath("~/Content/images/bacsi/" + fileName);

                    uploadAnhBia.SaveAs(path);
                    quanTri.AnhBia = fileName;
                }

                db.QuanTris.Add(quanTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }

        // GET: Admin/QuanTris/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }

        // POST: Admin/QuanTris/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDQuanTri,TaiKhoan,MatKhau,VaiTro,ThongTinBacSi,TrinhDo,IDKhoa,HoTen,AnhBia")] QuanTri quanTri,
                                 HttpPostedFileBase uploadAnhBia)
        {
            if (ModelState.IsValid)
            {
                var quantriDB = db.QuanTris.Find(quanTri.IDQuanTri);

                // Cập nhật thông tin text
                quantriDB.TaiKhoan = quanTri.TaiKhoan;
                quantriDB.MatKhau = quanTri.MatKhau;
                quantriDB.VaiTro = quanTri.VaiTro;
                quantriDB.ThongTinBacSi = quanTri.ThongTinBacSi;
                quantriDB.TrinhDo = quanTri.TrinhDo;
                quantriDB.IDKhoa = quanTri.IDKhoa;
                quantriDB.HoTen = quanTri.HoTen;

                // Nếu có upload ảnh mới
                if (uploadAnhBia != null && uploadAnhBia.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(uploadAnhBia.FileName);
                    string path = Server.MapPath("~/Common/images" + fileName);

                    uploadAnhBia.SaveAs(path);

                    // Xóa ảnh cũ (nếu có)
                    if (!string.IsNullOrEmpty(quantriDB.AnhBia))
                    {
                        string oldPath = Server.MapPath("~/Common/images" + quantriDB.AnhBia);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    // Lưu ảnh mới
                    quantriDB.AnhBia = fileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }

        // GET: Admin/QuanTris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: Admin/QuanTris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuanTri quanTri = db.QuanTris.Find(id);

            // Xóa ảnh khỏi thư mục
            if (!string.IsNullOrEmpty(quanTri.AnhBia))
            {
                string path = Server.MapPath("~/Common/images" + quanTri.AnhBia);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            db.QuanTris.Remove(quanTri);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
