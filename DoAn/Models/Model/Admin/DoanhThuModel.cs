using System.Runtime.Serialization;

namespace DoAn.Models.Model.Admin
{
    [DataContract]
    public class DoanhThuModel
    {
        public DoanhThuModel(string label, double y, string ngayban, int soluong)
        {
            Label = label;
            Y = y;
            NgayBan = ngayban;
            SoLuongBan = soluong;
        }

        public DoanhThuModel()
        {
            Label = "";
            Y = 0;
            NgayBan = "";
            SoLuongBan = 0;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")] public string Label { set; get; }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")] public double? Y { set; get; }

        public int SoLuongBan { set; get; }

        public string NgayBan { set; get; }
    }
}