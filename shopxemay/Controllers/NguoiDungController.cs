using System;
using System.Linq;
using System.Web.Mvc;
using shopxemay.Models;

namespace Tuan4_1911252687_ToTrungKien.Controllers
{
    public class NguoiDungController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: NguoiDung
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);
            
            if (string.IsNullOrEmpty(tendangnhap))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi3"] = "Email không được để trống";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi4"] = "Địa chỉ không được để trống";
            }
            if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi5"] = "Điện thoại không được để trống";
            }
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi6"] = "Họ tên không được để trống";
            }
            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["Loi7"] = "Phải nhập mật khẩu xác nhận";
            }
            if (string.IsNullOrEmpty(ngaysinh))
            {
                ViewData["Loi8"] = "Ngày sinh không được để trống";
            }
            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận không giống nhau";
                }
                else
                {
                    var check = data.KhachHangs.FirstOrDefault(s => s.tendangnhap == tendangnhap);
                    if (check == null)
                    {
                        kh.hoten = hoten;
                        kh.tendangnhap = tendangnhap;
                        kh.matkhau = matkhau;
                        kh.email = email;
                        kh.diachi = diachi;
                        kh.dienthoai = dienthoai;
                        kh.ngaysinh = DateTime.Parse(ngaysinh);
                        data.KhachHangs.InsertOnSubmit(kh);
                        data.SubmitChanges();
                        return RedirectToAction("DangNhap");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tài khoản đã tồn tại !!!";
                        return View();
                    }
                    
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == matkhau);
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}