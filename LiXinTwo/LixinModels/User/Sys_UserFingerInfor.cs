using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.User
{
    public class Sys_UserFingerInfor
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// UserId
        /// </summary>		
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// UserHrId
        /// </summary>		
        public string UserHrId
        {
            get;
            set;
        }
        /// <summary>
        /// Ssn
        /// </summary>		
        public string Ssn
        {
            get;
            set;
        }
        /// <summary>
        /// Name
        /// </summary>		
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// FingerTemplate
        /// </summary>		
        public byte[] FingerTemplate
        {
            get;
            set;
        }
        /// <summary>
        /// FingerTemplate1
        /// </summary>		
        public string FingerTemplate1
        {
            get;
            set;
        }
        /// <summary>
        /// FingerTemplate2
        /// </summary>		
        public string FingerTemplate2
        {
            get;
            set;
        }
    }
}
