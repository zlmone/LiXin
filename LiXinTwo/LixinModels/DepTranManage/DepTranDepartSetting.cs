using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepTranManage
{
    public class DepTranDepartSetting
    {
        #region model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// GroupName
        /// </summary>		
        public string DepartSetName
        {
            get;
            set;
        }
        /// <summary>
        /// UserIds
        /// </summary>		
        public string MainUserIDs
        {
            get;
            set;
        }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 用户数量
        /// </summary>
        public int UNumber { get; set; }

        public string CreateTimeStr
        {
            get { return CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男";
                else if (Sex == 1)
                    return "女";
                else
                    return "保密";
            }
        }

        /// <summary>
        /// DepartSetId
        /// </summary>
        public int DepartSetId { get; set; }
        
        public int totalcount { get; set; }
        /// <summary>
        /// 匹配标识（0 不匹配 1匹配）
        /// </summary>
        public int match { get; set; }
    }
}
