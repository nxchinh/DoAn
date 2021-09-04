using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class NhanVienDao
    {
        TraSuaEntities db = new TraSuaEntities();
        public DoAn.Models.EF.NhanVien getById(int? id)
        {
            return db.NhanViens.FirstOrDefault(x => x.Id == id);
        }
        public DoAn.Models.EF.NhanVien getByName(string name)
        {
            return db.NhanViens.FirstOrDefault(x => x.TenDangNhap == name);
        }
        public bool DangNhapNhanVien(string username, string password)
        {
            var result = db.NhanViens.Count(x => x.TenDangNhap == username && x.MatKhau == password);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}