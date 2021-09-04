using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Common.Function
{
    public class ConvertMoney
    {
        public int? ConvertTien(string money)
        {
            string[] moneySplit = money.Split(',');
            string lstmoney = "";
            foreach (var item in moneySplit)
            {
                lstmoney += item;
            }
            int? tien = Convert.ToInt32(lstmoney);
            return tien;
        }
    }
}