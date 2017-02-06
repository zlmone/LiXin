using System;
using System.Collections.Generic;
using LiXinModels.Survey;
using LiXinCommon;

namespace LiXinModels
{
    [Serializable]
    public partial class Message
    {
        #region model

        private string _coursename;

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// CourseName
        /// </summary>
        public string CourseName
        {
            get { return _coursename; }
            set { _coursename = value; }
        }
        /// <summary>
        /// Realname
        /// </summary>
        public string Realname
        {
            get;
            set;
        }
        /// <summary>
        /// LeaderID
        /// </summary>
        public string LeaderID
        {
            get;
            set;
        }
        /// <summary>
        /// Leadname
        /// </summary>
        public string Leadname
        {
            get;
            set;
        }
        /// <summary>
        /// Telephone
        /// </summary>
        public string MobileNum
        {
            get;
            set;
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get;
            set;
        }
        #endregion
    }

}

