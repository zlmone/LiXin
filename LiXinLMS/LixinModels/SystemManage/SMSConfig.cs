using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    [Serializable]
    public class SMSConfig
    {
        #region model

        public string ServiceID
        {
            get;
            set;
        }

        public string Account
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string hostIP
        {
            get;
            set;
        }

        public string PassWord
        {
            get;
            set;
        }


        public int Delay
        {
            get;
            set;
        }

        public int ResendTimeOut
        {
            get;
            set;
        }
        #endregion
    }
}
