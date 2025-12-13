using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;

namespace WebAppYte.Controllers
{
    public class NguoidungController : Controller
    {
        private modelWeb db = new modelWeb();

        // GET: Nguoidung
        public ActionResult Index()
        {
            var nguoiDungs = db.NguoiDungs.Include(n => n.GioiTinh).Include(n => n.Huyen);
            return View(nguoiDungs.ToList());
        }

        // GET: Nguoidung/DangKy
        public ActionResult DangKy()
        {
            // SỬA LỖI: Dùng "GioiTinh1" thay vì "GioiTinh"
            ViewBag.IDGioiTinh = new SelectList(db.GioiTinhs, "IDGioiTinh", nameof(WebAppYte.Models.GioiTinh.GioiTinh1));
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen");
            return View();
        }

        // POST: Nguoidung/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var check = db.NguoiDungs.FirstOrDefault(s => s.TaiKhoan == model.TaiKhoan);
                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.NguoiDungs.Add(model);
                    db.SaveChanges();
                    TempData["success"] = "Đăng ký thành công!";
                    return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ hoặc đăng nhập
                }
                else
                {
                    ViewBag.error = "Tài khoản đã tồn tại";
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                }
            }

            // SỬA LỖI: Load lại dropdown khi lỗi cũng phải dùng "GioiTinh1"
            ViewBag.IDGioiTinh = new SelectList(db.GioiTinhs, "IDGioiTinh", nameof(WebAppYte.Models.GioiTinh.GioiTinh1));
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen", model.IDHuyen);

            return View(model);
        }

        // GET: Nguoidung/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            // SỬA LỖI: Dùng "GioiTinh1"
            ViewBag.IDGioiTinh = new SelectList(db.GioiTinhs, "IDGioiTinh", nameof(WebAppYte.Models.GioiTinh.GioiTinh1));
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen", nguoiDung.IDHuyen);
            return View(nguoiDung);
        }

        // POST: Nguoidung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDNguoiDung,HoTen,Email,DienThoai,TaiKhoan,MatKhau,IDGioiTinh,DiaChiCuThe,SoCMND,IDHuyen,ThongTinKhac")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.capnhat = "Cập nhật thành công";
            }
            // SỬA LỖI: Dùng "GioiTinh1"
            ViewBag.IDGioiTinh = new SelectList(db.GioiTinhs, "IDGioiTinh", nameof(WebAppYte.Models.GioiTinh.GioiTinh1));
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen", nguoiDung.IDHuyen);
            return View(nguoiDung);
        }

        // GET: Nguoidung/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
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