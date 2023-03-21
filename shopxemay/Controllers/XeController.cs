using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopxemay.Models;
namespace shopxemay.Controllers
{
    public class XeController : Controller
    {
        // GET: Xe
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult ListXe()
        {
            var all_Xe = from ss in data.Xes select ss;
            return View(all_Xe);
        
        }
        public ActionResult Detail(int id)
        {
            var D_Xe = data.Xes.Where(m => m.maxe == id).First();
            return View(D_Xe);
        }
        public ActionResult XeSo()
        {
            var xeso = data.Xes.Where(n => n.maloai == 1); 
            return View(xeso);
        }
        public ActionResult XeTayGa()
        {
            var xetayga = data.Xes.Where(n => n.maloai == 2);
            return View(xetayga);
        }
        public ActionResult XeConTay()
        {
            var xecontay = data.Xes.Where(n => n.maloai == 3);
            return View(xecontay);
        }
        public ActionResult XeMoTo()
        {
            var xemoto = data.Xes.Where(n => n.maloai == 4);
            return View(xemoto);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Xe s)
        {
            var E_tenxe = collection["tenxe"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_tenxe))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tenxe = E_tenxe.ToString();
                s.hinh = E_hinh.ToString();
                s.giaban = E_giaban;
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                data.Xes.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("ListXe");
            }
            return this.Create();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/assets/images/" + file.FileName));
            return "/Content/assets/images/" + file.FileName;
        }
        public ActionResult Edit(int id)
        {
            var E_xe = data.Xes.First(m => m.maxe == id);
            return View(E_xe);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_xe = data.Xes.First(m => m.maxe == id);
            var E_tenxe = collection["tenxe"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            E_xe.maxe = id;
            if (string.IsNullOrEmpty(E_tenxe))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_xe.tenxe = E_tenxe;
                E_xe.hinh = E_hinh;
                E_xe.giaban = E_giaban;
                E_xe.ngaycapnhat = E_ngaycapnhat;
                E_xe.soluongton = E_soluongton;
                UpdateModel(E_xe);
                data.SubmitChanges();
                return RedirectToAction("ListXe");
            }
            return this.Edit(id);
        }

        public ActionResult Delete(int id)
        {
            var D_Xe = data.Xes.First(m => m.maxe == id);
            return View(D_Xe);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = data.Xes.Where(m => m.maxe == id).First();
            data.Xes.DeleteOnSubmit(D_sach);
            data.SubmitChanges();
            return RedirectToAction("ListXe");
        }

    }
}