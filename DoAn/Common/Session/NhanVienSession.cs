using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Common.Session
{
    [Serializable]
    public class NhanVienSession
    {
        public int Id { set; get; }

        public string UserName { set; get; }

        public int? MaChiNhanh { set; get; }
    }
}