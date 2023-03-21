using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SendMail;
using shopxemay.Models;

namespace Tuan4_1911252687_ToTrungKien.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();
                Session["GioHang"] = listGiohang;
            }
            return listGiohang;
        }

        public ActionResult ThemGiohang(int id, string strURL)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.Find(n => n.maxe == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.soluong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Sum(n => n.soluong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tt = listGiohang.Sum(n => n.dthanhtien);
            }
            return tt;

        }

        public ActionResult Giohang()
        {            
            List<GioHang> listGiohang = Laygiohang();
            //GioHang sanpham = listGiohang.SingleOrDefault(n => n.maxe == id);
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.maxe == id);
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.maxe == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> listGiohang = Laygiohang();
            
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.maxe == id);
            Xe xe = data.Xes.Where(n => n.maxe == sanpham.maxe).FirstOrDefault();
            if (sanpham != null)
            {
                if (Convert.ToInt32(collection["txtSoLuong"]) <= xe.soluongton)
                {
                    sanpham.soluong = Convert.ToInt32(collection["txtSoLuong"].ToString());
                }
                else if((Convert.ToInt32(collection["txtSoLuong"]) > xe.soluongton))
                {
                    ViewBag.ThongBao = "Sản phẩm vượt quá số lượng tồn";
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Xe s = new Xe();

            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.maxe = item.maxe;
                ctdh.soluong = item.soluong;
                ctdh.gia = (decimal)item.giaban;
                s = data.Xes.Single(n => n.maxe == item.maxe);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            SendEmail();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        public void SendEmail()
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            string content = System.IO.File.ReadAllText(Server.MapPath("~/SendMail/ThongTinDonHang.html"));
            content = content.Replace("{{CustomerName}}", kh.hoten);
            content = content.Replace("{{Phone}}", kh.dienthoai);
            content = content.Replace("{{Email}}", kh.email);
            content = content.Replace("{{Address}}", kh.diachi);
            content = content.Replace("{{Total}}", TongTien().ToString("N0"));

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(kh.email, "Đơn hàng mới từ XeHutech Shop", content);
            new MailHelper().SendMail(toEmail, "Đơn hàng mới từ XeHutech Shop", content);
        }

        public ActionResult XacnhanDonHang()
        {
            return View();
        }
    }
}
