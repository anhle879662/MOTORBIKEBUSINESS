using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using shopxemay.Models;

namespace shopxemay.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int maxe { get; set; }


        [Display(Name = "Tên xe ")]
        public string tenxe { get; set; }

        [Display(Name = "Ảnh bìa ")]
        public string hinh { get; set; }

        [Display(Name = "Giá bán ")]

        public Double giaban { get; set; }

        [Display(Name = "Số lượng ")]
        public int soluong { get; set; }
        [Display(Name = "Số lượng tồn ")]
        public int soluongton { get; set; }

        [Display(Name = "Thành tiền ")]
        public Double dthanhtien
        {
            get { return soluong * giaban; }
        }

        public GioHang(int id)
        {
            maxe = id;
            Xe sach = data.Xes.Single(n => n.maxe == maxe);
            tenxe = sach.tenxe;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            soluong = 1;
            soluongton = Convert.ToInt32(sach.soluongton);
        }
    }
}