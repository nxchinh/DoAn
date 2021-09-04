using System;

namespace DoAn.Models.Model.Admin
{
    public class PhanHoiModel
    {
        public int? Id { set; get; }

        public int? UserId { set; get; }

        public string UserName { set; get; }

        public string Content { set; get; }

        public int STT { set; get; }

        public int? DaXem { set; get; }

        public string PhanHoiTu { set; get; }

        public DateTime? ThoiGian { set; get; }
    }
}