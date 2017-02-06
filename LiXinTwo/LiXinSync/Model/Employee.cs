using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinSync.User
{
    [Serializable]
    public class Employee : BaseModel
    {
        public string Name
        {
            get;
            set;
        }

        public string type_name
        {
            get;
            set;
        }
    }
}
