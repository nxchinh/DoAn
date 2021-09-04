using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class DoanhThuDao
    {
        TraSuaEntities db = null;
        public DoanhThuDao()
        {
            db = new TraSuaEntities();
        }
        public int ngay_hoadon(DateTime? datetime1)
        {
            DateTime datetime = Convert.ToDateTime(datetime1);
            int thang_hoadon = datetime.Day;
            return thang_hoadon;
        }
        public int thang_hoadon(DateTime? datetime1)
        {
            DateTime datetime = Convert.ToDateTime(datetime1);
            int thang_hoadon = datetime.Month;
            return thang_hoadon;
        }
        public int nam_hoadon(DateTime? datetime1)
        {
            DateTime datetime = Convert.ToDateTime(datetime1);
            int thang_hoadon = datetime.Year;
            return thang_hoadon;
        }
        public int laysongaytrongthang(int thang, int nam)
        {
            int songay = 0;
            switch (thang)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: songay = 31; break;
                case 4:
                case 6:
                case 9:
                case 11: songay = 30; break;

                case 2:
                    if (nam % 400 == 0 || (nam % 4 == 0 && nam % 100 != 0))
                        songay = 29;
                    else
                        songay = 28;
                    break;
            }
            return songay;
        }
        public string dsngaytrongthang(int thang, int nam)
        {
            int songay = laysongaytrongthang(thang, nam);
            string lst = "";
            for (int i = 1; i <= songay; i++)
            {
                lst += i + "/" + thang + ',';
            }
            return lst;
        }
        public string ds_ngay_tru_thangfrom(int ngayfrom, int thang, int nam)
        {
            string lst = "";
            int songay = laysongaytrongthang(thang, nam);
            for (int i = ngayfrom; i <= songay; i++)
            {
                lst += i + "/" + thang + ',';
            }
            return lst;
        }
        public string ds_ngay_tru_thangto(int ngayto, int thang, int nam)
        {
            string lst = "";

            for (int i = 1; i <= ngayto; i++)
            {
                lst += i + "/" + thang + ',';
            }
            return lst;
        }
        public string ds_ngay_tru_trongthang(int ngayfrom, int ngayto, int thang, int nam)
        {
            string lst = "";

            for (int i = ngayfrom; i <= ngayto; i++)
            {
                lst += i + "/" + thang + ',';
            }
            return lst;
        }
    }
}