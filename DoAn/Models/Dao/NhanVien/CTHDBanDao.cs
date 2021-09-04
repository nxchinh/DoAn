using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NhanVien
{
    public class CTHDBanDao
    {
        TraSuaEntities db = new TraSuaEntities();
        public ChiTietHDB getById(int? machitiethd)
        {
            return db.ChiTietHDBs.FirstOrDefault(x => x.Id == machitiethd);
        }
        public int? Tien1LyTraSua(string mahoadon, int thuocsanpham, int? chitietthu)
        {
            var list = db.ChiTietHDBs.Where(x => x.ThuocSanPham == thuocsanpham && x.MaHDB == mahoadon && x.ChiTietThu == chitietthu).ToList();
            int? tien = 0;
            foreach (var item in list)
            {
                var product = new ProductDao().getByid(item.MaSanPham);
                tien += product.KhuyenMai;
            }
            return tien;
        }
        public string getMoTa(string mahoadon, int? chitietthu)
        {
            var mota = "";
            var list = db.ChiTietHDBs.Where(x => x.MaHDB == mahoadon && x.ChiTietThu == chitietthu).ToList();
            foreach (var item in list)
            {
                var categorydao = new CategoryDao();
                if (categorydao.getSPChinh(item.MaSanPham) != 1)
                {
                    mota += new ProductDao().getByid(item.MaSanPham).TenSanPham.ToString() + ", ";
                }

            }
            return mota;
        }
        public void Insert_CT(ChiTietHDB cthdb)
        {

            db.ChiTietHDBs.Add(cthdb);
            db.SaveChanges();
        }
        public List<ChiTietHDB> layDSSP(int productid, string mahoadon, int? chitietthu)
        {
            return db.ChiTietHDBs.Where(x => x.ThuocSanPham == productid && x.MaHDB == mahoadon && x.ChiTietThu == chitietthu).ToList();
        }
        public void Update(int cthdb_id, int? soluong)
        {
            var cthdb = db.ChiTietHDBs.Find(cthdb_id);
            cthdb.SoLuong = soluong;
            var product = new ProductDao().getByid(cthdb.MaSanPham);
            var giaban = product.KhuyenMai;
            cthdb.ThanhTien = soluong * giaban;
            db.SaveChanges();

        }
        public void Delete(int cthdb_id)
        {
            var cthdb = db.ChiTietHDBs.Find(cthdb_id);
            db.ChiTietHDBs.Remove(cthdb);
            db.SaveChanges();
        }
    }
}