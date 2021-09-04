using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Common.Session
{
    [Serializable]
    public class UserLogin
    {
        public int UserId { set; get; }
        public string UserName { set; get; }


    }
}