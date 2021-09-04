using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Common.Function
{
    public class LamTronThoiGian
    {
        public string LamTron(DateTime? now, DateTime? dateTime)
        {
            DateTime dtime1 = Convert.ToDateTime(now);
            DateTime dtime2 = Convert.ToDateTime(dateTime);
            TimeSpan timeSpan = dtime1 - dtime2;
            string lamtron = "";
            if (timeSpan.Days == 0)
            {
                if (timeSpan.Hours == 0)
                {
                    if (timeSpan.Minutes == 0)
                    {
                        lamtron = "vừa xong";
                    }
                    else
                    {
                        lamtron = "" + timeSpan.Minutes + " phút trước";
                    }
                }
                else
                {
                    lamtron = "" + timeSpan.Hours + " giờ trước";
                }
            }
            else
            {
                lamtron = "" + timeSpan.Days + " ngày trước";
            }

            return lamtron;
        }
    }
}