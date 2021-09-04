using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn.Common.Session
{
    [Serializable]
    public class UserRegister
    {
        public int Code { set; get; } 
        public int Id { get; set; }
    }
}
