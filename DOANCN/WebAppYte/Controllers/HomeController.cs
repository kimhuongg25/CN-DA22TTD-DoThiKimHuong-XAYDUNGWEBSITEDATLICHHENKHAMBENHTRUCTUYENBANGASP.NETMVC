using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;

namespace WebAppYte.Controllers
{
    public class HomeController : Controller
    {
        modelWeb db = new modelWeb();

        public ActionResult Index()
        {
            var tintuc = db.Tintucs.ToList();
            return View(tintuc);
        }

        public ActionResult Trangchu()
        {
            return View();
        }

        // ===========================
        // ĐĂNG KÝ
        // ===========================
        [HttpGet]
        public ActionResult Dangky()
        {
            // Đã đúng
            ViewBag.IDGioiTinh = new SelectList(db.GioiTinhs, "IDGioiTinh", "GioiTinh1");
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dangky([Bind(Include = "IDNguoiDung,HoTen,Email,DienThoai,TaiKhoan,MatKhau,IDGioiTinh,DiaChiCuThe,SoCMND,IDHuyen,ThongTinKhac")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng tài khoản (Nên thêm đoạn này để tránh lỗi SQL)
                var check = db.NguoiDungs.FirstOrDefault(s => s.TaiKhoan == nguoiDung.TaiKhoan);
                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.NguoiDungs.Add(nguoiDung);
                    db.SaveChanges();
                    Session["nguoidung"] = nguoiDung;
                    return RedirectToAction("Dangnhap");
                }
                else
                {
                    ViewBag.error = "Tài khoản đã tồn tại";
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                }
            }

            // --- SỬA LỖI Ở ĐÂY: Thêm số 1 vào "GioiTinh1" ---
            ViewBag.IDGioiTinh = new SelectList(db.GioiTinhs, "IDGioiTinh", "GioiTinh1", nguoiDung.IDGioiTinh);
            ViewBag.IDHuyen = new SelectList(db.Huyens, "IDHuyen", "TenHuyen", nguoiDung.IDHuyen);

            return View(nguoiDung);
        }

        // ===========================
        // ĐĂNG NHẬP
        // ===========================
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection Dangnhap)
        {
            string tk = Dangnhap["TaiKhoan"];
            string mk = Dangnhap["MatKhau"];

            var islogin = db.NguoiDungs.SingleOrDefault(x => x.TaiKhoan == tk && x.MatKhau == mk);
            var isloginAdmin = db.QuanTris.SingleOrDefault(x => x.TaiKhoan == tk && x.MatKhau == mk);

            if (islogin != null)
            {
                Session["user"] = islogin;
                // Chuyển hướng sang xem chi tiết người dùng
                return RedirectToAction("Details", "Nguoidung", new { id = islogin.IDNguoiDung });
            }
            else if (isloginAdmin != null && isloginAdmin.VaiTro == 1) // Quản trị viên
            {
                Session["userAdmin"] = isloginAdmin;
                // Lưu ý: Nếu Admin nằm trong Area thì phải thêm new { area = "Admin" }
                // Giả sử Controller tên là HomeAdmin trong Area Admin
                return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
            }
            else if (isloginAdmin != null && isloginAdmin.VaiTro == 2) // Bác sĩ
            {
                Session["userBS"] = isloginAdmin;
                return RedirectToAction("Trangchu", "Home");
            }
            else
            {
                ViewBag.Fail = "Tài khoản hoặc mật khẩu không chính xác.";
                return View("Dangnhap");
            }
        }

        // ===========================
        // ĐĂNG XUẤT
        // ===========================
        public ActionResult DangXuat()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangXuatBs()
        {
            Session["userBS"] = null;
            return RedirectToAction("Index", "Home");
        }

        // ===========================
        // TIN TỨC
        // ===========================
        public ActionResult TintucNewPartial()
        {
            var tintuc = db.Tintucs.Where(x => x.TheLoai == "new");
            return PartialView(tintuc);
        }

        public ActionResult TintucHotPartial()
        {
            var tintuc = db.Tintucs.Where(x => x.TheLoai == "hot");
            return PartialView(tintuc);
        }

        public ActionResult Xemchitiet(int IDTintuc = 0)
        {
            var chitiet = db.Tintucs.SingleOrDefault(n => n.IDTintuc == IDTintuc);
            if (chitiet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitiet);
        }
    }
}