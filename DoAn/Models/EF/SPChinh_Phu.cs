using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models.EF
{
    public class SPChinh_Phu
    {
        public int Id { get; set; }

        [Column("SPChinh_Phu")]
        [StringLength(50)]
        public string SPChinh_Phu1 { get; set; }
    }
}