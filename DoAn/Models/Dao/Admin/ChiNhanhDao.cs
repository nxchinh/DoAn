using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class ChiNhanhDao
    {
        TraSuaEntities db = null;
        public ChiNhanhDao()
        {
            db = new TraSuaEntities();
        }

        public ChiNhanh getByid(int? machinhanh)
        {
            return db.ChiNhanhs.SingleOrDefault(x => x.Id == machinhanh);
        }
        public ChiNhanh FindSanPham(int? machinhanh)
        {
            return (db.ChiNhanhs.Find(machinhanh));
        }
        public int AddChiNhanh(ChiNhanhModel cn)
        {
            var chinhanh = new ChiNhanh();
            
            chinhanh.TenChiNhanh =cn.TenChiNhanh;
            chinhanh.DiaChi = cn.DiaChi;
            
            db.ChiNhanhs.Add(chinhanh);
            db.SaveChanges();
            return chinhanh.Id;
        }
        public int updateChiNhanh(ChiNhanhModel cn)
        {
            var chinhanh = db.ChiNhanhs.Find(cn.Id);
           
            chinhanh.TenChiNhanh= cn.TenChiNhanh;
            chinhanh.DiaChi = cn.DiaChi;
           
            db.SaveChanges();
            return chinhanh.Id;
        }
        public int Delete(int machinhanh)
        {
            var chinhanh = db.ChiNhanhs.Find(machinhanh);
            db.ChiNhanhs.Remove(chinhanh);
            db.SaveChanges();
            return chinhanh.Id;
        }
    }
}