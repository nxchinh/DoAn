using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Common.Session
{
    [Serializable]
    public class SanPhamThuSession
    {
        public int Id { set; get; }

        public int SanPham_Thu { set; get; }
    }
}