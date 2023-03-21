    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopxemay.Models;
using PagedList;


namespace shopxemay.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index(int? page, string name)
        {
            int pageSize = 5;
            int pageNum = page ?? 1;
            ViewBag.Find = name;
            if (page == null) page = 1;
            var all_sach = (from ele in data.Xes select ele).OrderBy(p => p.maxe);
            if (!String.IsNullOrEmpty(name))
            {
                all_sach = (IOrderedQueryable<Xe>)all_sach.Where(a => a.tenxe.Contains(name));
                return View(all_sach.ToList().ToPagedList(pageNum, pageSize));
            }
            return View(all_sach.ToList().ToPagedList(pageNum, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}