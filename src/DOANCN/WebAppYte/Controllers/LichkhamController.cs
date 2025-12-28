using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;
using PagedList;

namespace WebAppYte.Controllers
{
    public class LichkhamController : Controller
    {
        private modelWeb db = new modelWeb();

        // === 1. DANH SÁCH TẤT CẢ ===
        public ActionResult Index(int? page)
        {
            // Kiểm tra đăng nhập
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null) return RedirectToAction("DangNhap", "NguoiDung");

            // Lấy ID từ Session thay vì từ URL để bảo mật
            int id = u.IDNguoiDung;

            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri)
                .Where(h => h.IDNguoiDung == id)
                .OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }

        // === 2. ĐANG XỬ LÝ (Trạng thái 0) ===
        public ActionResult Dangxuly(int? page)
        {
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null) return RedirectToAction("DangNhap", "NguoiDung");

            int id = u.IDNguoiDung;

            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri)
                .Where(h => h.IDNguoiDung == id && h.TrangThai == 0)
                .OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }

        // === 3. ĐÃ XÁC NHẬN (Trạng thái 1) ===
        public ActionResult Daxacnhan(int? page)
        {
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null) return RedirectToAction("DangNhap", "NguoiDung");

            int id = u.IDNguoiDung;

            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri)
                .Where(h => h.IDNguoiDung == id && h.TrangThai == 1)
                .OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }

        // === 4. ĐÃ HOÀN THÀNH (Trạng thái 2) ===
        public ActionResult Datuvanxong(int? page)
        {
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null) return RedirectToAction("DangNhap", "NguoiDung");

            int id = u.IDNguoiDung;

            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri)
                .Where(h => h.IDNguoiDung == id && h.TrangThai == 2)
                .OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }

        // GET: Lichkham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null) return HttpNotFound();
            return View(lichKham);
        }

        // GET: Lichkham/Create
        public ActionResult Create()
        {
            // Kiểm tra đăng nhập trước khi cho đặt lịch
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null) return RedirectToAction("DangNhap", "NguoiDung");

            // Chỉ cần truyền danh sách Bác sĩ (VaiTro == 2)
            // Không cần truyền danh sách User vì người đặt chính là người đang login
            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(n => n.VaiTro == 2), "IDQuanTri", "HoTen");

            return View();
        }

        // POST: Lichkham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,DiaChi,KetQuaKham,IDQuanTri")] LichKham lichKham)
        {
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null) return RedirectToAction("DangNhap", "NguoiDung");

            if (ModelState.IsValid)
            {
                // Gán tự động các giá trị hệ thống
                lichKham.IDNguoiDung = u.IDNguoiDung; // Người đặt là người đang login
                lichKham.TrangThai = 0; // Mới tạo thì auto là Chờ xử lý

                db.LichKhams.Add(lichKham);
                db.SaveChanges();
                // Chuyển hướng về trang Đang xử lý
                return RedirectToAction("Dangxuly");
            }

            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(n => n.VaiTro == 2), "IDQuanTri", "HoTen", lichKham.IDQuanTri);
            return View(lichKham);
        }

        // GET: Lichkham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null) return HttpNotFound();

            // Check quyền: Chỉ chủ sở hữu mới được sửa (hoặc admin - tùy bạn)
            var u = Session["user"] as WebAppYte.Models.NguoiDung;
            if (u == null || lichKham.IDNguoiDung != u.IDNguoiDung)
            {
                // Nếu không phải chính chủ, không cho sửa
                return RedirectToAction("Index");
            }

            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(n => n.VaiTro == 2), "IDQuanTri", "HoTen", lichKham.IDQuanTri);
            return View(lichKham);
        }

        // POST: Lichkham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,DiaChi,KetQuaKham,IDNguoiDung,IDQuanTri")] LichKham lichKham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichKham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dangxuly"); // Sửa xong quay về danh sách đang xử lý
            }
            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(n => n.VaiTro == 2), "IDQuanTri", "HoTen", lichKham.IDQuanTri);
            return View(lichKham);
        }

        // GET: Lichkham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null) return HttpNotFound();
            return View(lichKham);
        }

        // POST: Lichkham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LichKham lichKham = db.LichKhams.Find(id);
            db.LichKhams.Remove(lichKham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Lichdangluoi()
        {
            List<LichKham> l = db.LichKhams.ToList();
            var events = l.Select(ll => new
            {
                id = ll.IDLichKham,
                title = ll.ChuDe,
                start = ll.BatDau,
                end = ll.KetThuc,
            });
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult lichhen()
        {
            return View();
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